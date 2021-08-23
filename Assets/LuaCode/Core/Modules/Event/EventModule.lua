---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 18:56:57
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class ListenerModule
local EventModule = {}
local meta = {__index = EventModule}
local _addListener
local _removeListener

function EventModule.Create(owner, mod)
	mod.listeners_ = {}
	setmetatable(mod, meta)
	mod.gameName_ = Game.Current()
end

function EventModule.Destroy(mod)
	local remove = Dispatch.SpanRemove
	for k, v in pairs(mod.listeners_) do
		remove(self.gameName_, v)
	end
	mod.listeners_ = nil
end

function EventModule:Add(etype, handler)
	if self.listeners_[etype] then
		error("已经添加过该类型的监听,不允许重复添加")
	end
	local handle = Dispatch.SpanAdd(self.gameName_, etype, handler)
	self.listeners_[etype] = handle
end

function EventModule:Send(etype, ...)
	Dispatch.SpanSend(self.gameName_, etype, ...)
end

function EventModule:Remove(etype)
	local handle = self.listeners_[etype]
	if handle then
		Dispatch.SpanRemove(self.gameName_, handle)
		self.listeners_[etype] = nil
	end
end

return EventModule