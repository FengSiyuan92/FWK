---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-26 16:55:04
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class SpriteSetter
local UIHelper = {}

local meta = {__index = UIHelper}
local AssetManager = CS.AssetManager

function UIHelper.Create(owner, mod)
	mod.owner_ = owner
	mod.spriteState_ = {}
	setmetatable(mod, meta)
end

function UIHelper.Destroy(mod)
	mod.spriteState_ = nil
end

function UIHelper:SetSprite(imageNodeName, spriteName)
	-- 获取节点
	local image = self.owner_.cs[imageNodeName]:GetImage()
	if not image then
		error("没有" .. imageNodeName .. "节点")
		return
	end
	
	local instanceId = image:GetInstanceID()
	self.spriteState_[instanceId] = spriteName
	
	self.owner_.asset:LoadSprite(spriteName, function(sprite)
		if self.spriteState_[instanceId] ~= spriteName then
			return
		end
			LogRed("设置sprite" .. spriteName)
		image.sprite = sprite
	end)
end

return UIHelper