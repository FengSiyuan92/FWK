---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-23 20:43:11
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class Asset
local AssetUser = {}
local meta = {__index = AssetUser}
local CSAction = CS.FAction.Action

function AssetUser.Create(owner, mod)
	self.owner_ = owner
	setmetatable(mod, meta)
end

function AssetUser.Destroy(mod)
	
end

function AssetUser:GetSprite()
	
end

function AssetUser:GetSprite()

end

return AssetUser