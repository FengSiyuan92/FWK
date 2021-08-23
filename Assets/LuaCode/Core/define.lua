---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 15:48:03
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class deine
require('LuaDebuggee').StartDebug('127.0.0.1', 9826)

-- 全局工具类函数
require "Core.Base.class"
require "Core.gc"
require "Core.Util.json"
require "Core.Util.log"
require "Core.Util.utf8"
require "Core.Util.stringutil"
require "Core.Util.tableutil"


-- CS常用工具类
Action = CS.FAction.Action




-- 全局工具类对象
LocalData = require "Core.Util.LocalData"
NumberUtil = require "Core.Util.NumberUtil"
Time = require "Core.Util.Time"

-- 模块组件
Object = require "Core.Object"
Game = require "Core.Game"
Dispatch = require "Core.Dispatch.Dispatch"

