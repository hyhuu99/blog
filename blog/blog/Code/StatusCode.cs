using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blog.Code
{
    public enum StatusCode
    {
        ACTIVE = 1,
        DISACTIVE = 0,
        ISEXISTSUSER = -1,
        DELETE = -2,
        FAIL = -3,

        MAX_WIDTH = 600,
        MAX_HEIGHT = 600,

        UNAUTHORIZED=401,
        SUCCESS = 200,
        WASDELETED = 1001,
        TITLEWASCHANGED = 1002,
        CONTENTWASCHANGED = 1003,
        TAGWASCHANGED = 1004,
    }
}