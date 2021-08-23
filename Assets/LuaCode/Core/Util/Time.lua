
local GetString = function () end
local curTime = 0

local unity_time = CS.UnityEngine.Time

local servertime = 0;
local stime = 0;
local clientToServer = 0

local Time = {}

local function getTimeZone()
	local now = os.time()
	local utc = os.date("!*t", now)
	local utftime = os.time(utc)
	local diff = os.difftime(now, utftime)
	return diff / 3600
end

-- 获得服务器时区
function Time.GetZoneDelta()
	return clientToServer
end

-- 从程序启动以来的真实时间
function Time.GetAppRunTime()
	return unity_time.realtimeSinceStartup
end

function Time.SetCurrentTime(timestamp)
	servertime = timestamp
	stime = unity_time.realtimeSinceStartup

	local serverZone = ClientData:GetInstance().zone
	local clientZone = getTimeZone()
	clientToServer = (serverZone - clientZone) * 3600
end

function Time.GetCurrentTime()
	local timeoffet = unity_time.realtimeSinceStartup - stime
	if(servertime == 0)then
		return os.time()
	else
		return servertime + math.floor(timeoffet)
	end
end

function Time.GetServerZoneDate(timestamp)
	local zoneTime= (timestamp or TimerUtil.GetCurrentTime())+TimerUtil.GetZoneDelta()
	local date = os.date("*t", zoneTime)
	date.minute = date.min
	date.second = date.sec
	return date
end

function Time.GetServerTimeByDate(dateTable)
	local time = os.time(dateTable) - TimerUtil.GetZoneDelta()
	return time
end



function Time.GetOfflineStr(lasttimestamp)
	if not lasttimestamp or lasttimestamp == -1 then
		return GetString("S_10002")
	end

	local currenttimestamp = TimerUtil.GetTimeStamp()
	local delta = currenttimestamp - lasttimestamp
	local lasttime = TimerUtil.GetDate(lasttimestamp)

	local displayOffliine = ""
	-- -- TODO: 测试代码,打印出离线时间
	--displayOffliine = string.format("%d/%d/%d %d:%d:%d",lasttime.year, lasttime.month,lasttime.day, lasttime.hour, lasttime.minute, lasttime.second)

	-- 如果小于1小时，显示刚刚离线
	local offlineHour = math.floor(delta / 3600)
	if offlineHour < 1 then
		return displayOffliine .. GetString("S_10003")
	end

	-- 如果小于一天，显示离线多少小时
	local offlineDay = math.floor(offlineHour / 24)
	if offlineDay < 1 then
		return displayOffliine .. formatCS(GetString("S_10004"), offlineHour)
	end

	local currenttime = TimerUtil.GetDate()
	local year = currenttime.year - lasttime.year
	local month = currenttime.month - lasttime.month
	local day = currenttime.day - lasttime.day
	local hour = currenttime.hour - lasttime.hour

	-- 如果小于一个自然月，则显示离线多少天
	local offlineMonth = math.floor(year * 12 + month + (day >= 0 and 0 or -1))
	if offlineMonth < 1 then
		return displayOffliine .. formatCS(GetString("S_10005"), offlineDay)
	end

	-- 如果小于一个自然年，则显示离线多少个月
	local offlineYear = math.floor(year + ((month >= 0 and day >= 0) and 0 or -1))
	if offlineYear < 1 then
		return displayOffliine .. formatCS(GetString("S_10006"), offlineMonth)
	end

	return displayOffliine .. formatCS(GetString("S_10007"), offlineYear)
	-- return offlineTime
end

return Time