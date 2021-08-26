---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 15:48:12
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class main
local main = {}

Game.Open("hall")


local t = class("Test")
t.Mod = {Mod.cs, Mod.ui, Mod.asset}
function t:OnCreate()
	self.gameObject = CS.UnityEngine.GameObject.Find("RuntimeDebug")
end


function main.Start() 
	local ins = t.New()
	local b = 0
	ins.ui:SetSprite("TestImg", "activeequipment_img_ssr_1")
	ins.ui:SetSprite("TestImg", "testimg1")

	ins:Destroy()
	--ins.cs.TestImg.sprite = ins.ui:GetSprite("activeequipment_img_ssr_1")
end

return main