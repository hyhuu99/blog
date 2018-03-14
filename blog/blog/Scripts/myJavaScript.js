
var dynamicId = 0

$("#tag-input").on('keydown', function (e) {
    if (e.which === 13 || e.keyCode === 13) {
        dynamicId += 1;
        $('#detail').append(
            "<li>" +
            '<button class="btn btn-info">' + $('#tag-input').val() + '</button>'+
            '<input type="hidden" name="post.NameTag[' + (dynamicId - 1) + ']" value="' + $('#tag-input').val() + '" />' +
            '<span class="token-input-delete-token">×</span>'+
            "</li>"
        )
        
        return false;
    }
});

$("#detail").on('click', '.btn.btn-info', function () {
    $(this).closest('li').find("input[type='hidden']").remove();
    $(this).closest('li').remove();
});

//$("#comment-button").click( function () {
//    $('textarea#CommentContent').val(" ");
//    console.log("test");
//});

$(function () {
    $('input[type=file]').change(function () {
        var val = $(this).val().toLowerCase(),
            regex = new RegExp("(.*?)\.(jfif|jpg|jpeg|png|bmp)$");
        if (!(regex.test(val))) {
            $(this).val('');
            alert('Please select correct file format');
        }
    });
});
$(".delete-comment-button").click( function () {
    var id = $(this);
    $.ajax({
        url: "/Comments/Delete",
        type: "POST",
        ddataType: "json",
        data: {
            value: id.val()
        },
        success: function (result) {
            if (result===200)
                $("#comment_" + id.val()).remove();
            else {
                if (result === 401)
                    alert('UNAUTHORIZED!');
            }
        },
        error: function () {
            alert('Can not delete your comment !');
        }
    })
});

$(document).ready(function () {
    $("#tag-input").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Tags/AutoCompleteTag",
                type: "POST",
                ddataType: "json",
                data: {
                    value: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Name +" "+ item.Frequency,
                            value: item.Name,
                        };
                    }))
                },
                error: function () {
                    alert('something went wrong !');
                }
            })
        },
        open: function () {
            $('.ui-autocomplete').width('-18em');
            $('.ui-widget-content').css('background', '#c0c0c0', 'border: 1px solid black');
            $('.ui-menu-item a').removeClass('ui-corner-all');
        }
    })
}) 

$(document).ready(function () {
    $("#txtSearch").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Posts/AutoCompleSearch",
                type: "Get",
                dataType: "json",
                data: {
                    value: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            title: item.Title,
                            slug: item.Slug,                          
                            image: item.Image,
                            postId: item.PostId
                        };
                    }))
                },
                error: function () {
                    alert('something went wrong !');
                }
            })
        },
        messages: {
            noResults: "",
            results: ""
        },
        open: function () {
            $('.ui-autocomplete').width('-18em');
            $('.ui-widget-content').css('background', '#E1F7DE', 'border: 1px solid black');
            $('.ui-menu-item a').removeClass('ui-corner-all');
        }
    })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append(
                "<img src=" + item.image + " " + "style='width:3em;height:3em;margin:0.2em 0.3em 1em 0'>" +
                "<a href=/Posts/" + item.postId + "/" + item.slug + ">" +
                item.title
                )
                .appendTo(ul);
        };
}) 

