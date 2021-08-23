
--[[
客户端本地化数据存储接口.
封装CS的PlayerPrefs. 支持存储Lua侧支持的number,boolean,string三种基本数据类型.
1.支持lua语法. 如 local bFalse = nil   LocalDataUtil.SetBoolean("testKey", bTrue), 取出再用时取出的为false
2.解决numer和float精度问题. 如直接调用CS.UnityEngine.PlayerPrefs.SetFloat("key", 0.2), 取出来不一定就是多少了 0.1999999...
对于lua侧的值类型有时候可能是整数或者浮点数,不用再区分到底是int还是float,直接存储number即可,取出时可以对应之前的值
3.区分客户端本地存储和玩家存储两个大块
客户端存储:对应手机上的数据,不换手机不清包数据,就有值,不同的playerid不会影响该数据.
玩家存储:对应不同playerid的数据,采用playerid..key值得方式进行存储.玩家更换账号就会有新的数据

对应接口函数名   Set / Get   ..  * / Player   .. String / Boolean /Number  排列组合即可. *表示没有内容,默认为客户端数据
设置接口的参数为1. key  2.value    获取接口的参数为  1. key   可选 defaultValue
]]

local LocalData = {}
local PlayerPrefs = CS.UnityEngine.PlayerPrefs

local combineKey

function LocalData.SetString(key, value)
	if not value or type(value) ~= "string" then return end
	key = tostring(key)
	PlayerPrefs.SetString(key, value)
end

function LocalData.GetString(key, defaultValue)
	defaultValue = defaultValue or ""
	key = tostring(key)
	return PlayerPrefs.GetString(key, defaultValue)
end

function LocalData.SetBoolean(key, value)
	value = value and 1 or 0
	key = tostring(key)
	PlayerPrefs.SetInt(key, value)
end

function LocalData.GetBoolean(key, defaultValue)
	key = tostring(key)
	defaultValue = defaultValue and 1 or 0
	local value = PlayerPrefs.GetInt(key, defaultValue)
	return value == 1
end

function LocalData.SetNumber(key, value)
	if not value or type(value) ~= "number" then return end
	key = tostring(key)
	value = tostring(value)
	PlayerPrefs.SetString(key, value)
end

function LocalData.GetNumber(key, defaultValue)
	defaultValue = defaultValue or 0
	key = tostring(key)
	local value =  PlayerPrefs.GetString(key, tostring(defaultValue))
	return tonumber(value)
end

function LocalData.SetPlayerString(key, value)
	if not value or type(value) ~= "string" then return end
	key = combineKey(tostring(key))
	PlayerPrefs.SetString(key, value)
end

function LocalData.GetPlayerString(key, defaultValue)
	defaultValue = defaultValue or ""
	key = combineKey(tostring(key))
	return PlayerPrefs.GetString(key, defaultValue)
end

function LocalData.SetPlayerBoolean(key, value)
	value = value and 1 or 0
	key = combineKey(tostring(key))
	PlayerPrefs.SetInt(key, value)
end

function LocalData.GetPlayerBoolean(key, defaultValue)
	key = combineKey(tostring(key))
	defaultValue = defaultValue and 1 or 0
	local value = PlayerPrefs.GetInt(key, defaultValue)
	return value == 1
end

function LocalData.SetPlayerNumber(key, value)
	if not value or type(value) ~= "number" then return end
	key = combineKey(tostring(key))
	value = tostring(value)
	PlayerPrefs.SetString(key, value)
end

function LocalData.GetPlayerNumber(key, defaultValue)
	defaultValue = defaultValue or 0
	key = combineKey(tostring(key))
	local value =  PlayerPrefs.GetString(key, tostring(defaultValue))
	return tonumber(value)
end

function combineKey(key)
	return ClientData:GetInstance():GetPlayerId() .. key
end


function LocalData.HasKey(key)
	return  PlayerPrefs.HasKey(key)
end

return LocalData