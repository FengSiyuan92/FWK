---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 18:56:57
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class ListenerModule
local ListenerModule = class("ListenerModule", BaseModule)

function ListenerModule:OnCreate()
	self.listeners_ = {}
end

function ListenerModule:OnDestroy()
	for k, v in pairs(self.listeners_) do
		self:Remove(k)
	end
end

function ListenerModule:Add(etype, handler)
	
end

function ListenerModule:Send(etype, ...)
	
end

function ListenerModule:Remove()
	
end

return ListenerModule