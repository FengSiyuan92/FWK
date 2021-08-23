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
t.Mod = {Mod.event}
function t:OnCreate()
end

function main.Start() 
	local e = "Test"
	local test = t.New()
	test.event:Add(e, function(a)
			local p = a
		local b = 0	
	end)
	
	test.event:Send(e, 10)
	
	
	
	Game.Back()
	
	test.event:Send(e, 10)
end



return main