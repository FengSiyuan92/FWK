--[[
-- added by Golden @ 2017-11-30
-- Logger系统：Lua中所有错误日志输出均使用本脚本接口，以便上报服务器
--]]

local Logger = {}

local Macro = Macro

local function CanPrintLog()
	return true
end
local Logger = CS.UnityEngine.Debug

function Log(msg)
	if not CanPrintLog() then
		return
	end
	Logger.Log(debug.traceback(msg, 2))
end

function LogObj(msg,obj)
	if not CanPrintLog() then
		return
	end
	Logger.Log(debug.traceback(msg, 2),obj)
end

function LogError(msg)
	Logger.LogError(debug.traceback(msg, 2))
end

function LogErrorObj(msg,obj)
	Logger.LogError(debug.traceback(msg, 2),obj)
end

function LogWarning(msg)
	if not CanPrintLog() then
		return
	end
	Logger.LogWarning(debug.traceback(msg, 2))
end

function LogRed(str)
	if not CanPrintLog() then
		return
	end
	Logger.Log(debug.traceback(string.format("<color=Red>%s</color>",str),2))
end

function LogReceiveMsg(msg_id,msgName,msg)
	if not CanPrintLog() then
		return
	end
	Logger.Log("<color=red>Receive----msg_id = "..msg_id.." msgName  = [" .. msgName.."]</color>"..serpent.block(msg))
end

function LogSendMsg(msg_id,msgName,msg)
	if not CanPrintLog() then
		return
	end
	Logger.Log("<color=green>send msg :" .. msg_id.." codeName = ["..CodeOpMap[msg_id].."]</color>"..serpent.block(msg))
end


return Logger