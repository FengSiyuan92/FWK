---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-23 20:49:32
---------------------------------------------------------------------

-- action托管器,调用Hold将创建的action托管进这里,在Object销毁的时候,会自动终止行为
-- 防止调用某些无法继续执行的逻辑,也防止自己控制action的生命周期

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