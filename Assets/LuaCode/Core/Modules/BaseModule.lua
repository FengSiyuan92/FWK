---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 18:59:51
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class BaseModule
local BaseModule = {}

function BaseModule:OnCreate(owner)
	self.owner_ = owner
end

function BaseModule:OnDestroy()
	self.owner_ = nil
end

return BaseModule