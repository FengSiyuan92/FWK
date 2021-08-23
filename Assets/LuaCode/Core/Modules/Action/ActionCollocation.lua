---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-23 20:49:32
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class ActionCollocation
local ActionCollocation = {}
local meta = {__index = ActionCollocation}
local CSAction = CS.FAction.Action

function ActionCollocation.Create(owner, mod)
	self.owner_ = owner
	setmetatable(mod, meta)
end

function ActionCollocation.Destroy(mod)
	for index, action in pairs(self.actions_) do
		action:Destroy()
	end
end

function ActionCollocation:Hold(action)
	if not self.actions_ then
		self.actions_ = {}
	end
	
	self.actions_[#self.actions_ + 1] = action
end


return ActionCollocation