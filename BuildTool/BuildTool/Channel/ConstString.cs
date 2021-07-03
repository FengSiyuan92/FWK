﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
{
    internal static class ConstString
    {
        // 编译用字符串常量
        public const string STR_ALIAS = "alias";
        public const string STR_KEY = "key";
        public const string STR_DEFAULT = "default";
        public const string STR_C = "c";
        public const string STR_S = "s";
        public const string STR_CS = "cs";
        public const string STR_SC = "sc";
        public const string STR_EQ = "=";
        public const string STR_EMPTY = "";
        public const string STR_LIST_END = "[]";

        // EXCEL 使用的中文定义字符串
        public const string VALID_SHEET_SIGN = "**";
        public const string EXL_FIELD_NAME_TITLE = "字段名";
        public const string EXL_FIELD_TYPE_TITLE = "类型";
        public const string EXL_FIELD_OUTPUT_TYPE_TITLE = "导出";
        public const string EXL_FIELD_APPEND_TITLE = "内容描述";
        public const string EXL_CHECK_RULE_TITLE = "检查规则";

        // XML使用的定义字符串
        public const string XML_TYPE_TITLE = "type";
        public const string XML_NAME_TITLE = "name";
        public const string XML_VALUE_TITLE = "value";
        public const string XML_DESC_TITLE = "desc";
        public const string XML_CHECK_RULE_TITLE = "check";
        public const string XML_OUTPUT_TYPE_TITLE = "out";
        public const string XML_ALIAS_TITLE = "alias";
        public const string XML_ENUM_DEF_TYPE = "enum";
        public const string XML_OBJ_DEF_TYPE = "obj";
    }
}