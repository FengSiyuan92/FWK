---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 19:02:02
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class Dispatch
local Dispatch = {}
local Dispatcher = require "Core.Dispatch.EventDispatcher"
local DispatcherCache = {}

function Dispatch.RequestGameDispatcher(gameName)
	local old = DispatcherCache[gameName]
	if not old then
		old = Dispatcher.New()
		DispatcherCache[gameName] = old
	end
	return old
end

function Dispatch.CloseDispatcher(gameName)
	local old = DispatcherCache[gameName]
	if old then
		old:Destroy()
		DispatcherCache[gameName] = nil
	end
end

function Dispatch.Send(etype, ...)
	local gameName = Game.Current()
	local dispatcher = DispatcherCache[gameName]
	if dispatcher then
		dispatcher:Send(etype, ...)
	end
end

function Dispatch.SpanSend(gameName, etype, ...)
	local dispatcher = DispatcherCache[gameName]
	if dispatcher then
		dispatcher:Send(etype, ...)
	end
end

function Dispatch.Add(etype, handler)
	local gameName = Game.Current()
	local dispatcher = DispatcherCache[gameName]
	if dispatcher then
		return dispatcher:Add(etype, handler)
	end
end

function Dispatch.SpanAdd(gameName, etype, handler)
	local dispatcher = DispatcherCache[gameName]
	if dispatcher then
		return dispatcher:Add(etype, handler)
	end
end

function Dispatch.Remove(handlerId)
	local gameName = Game.Current()
	local dispatcher = DispatcherCache[gameName]
	if dispatcher then
		return dispatcher:Remove(handlerId)
	end
end

function Dispatch.SpanRemove(gameName, handlerId)
	local dispatcher = DispatcherCache[gameName]
	if dispatcher then
		return dispatcher:Remove(handlerId)
	end
end



return Dispatch