---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 17:21:10
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class gc


local function setmt_gc(t, mt)
	local prox = newproxy(true)
	getmetatable(prox).__gc = function()  mt.__gc(t) end
	t[prox] = true
	return setmetatable(t, mt)
end

function attachGC(tab, func)
	setmt_gc(tab, {__gc = func})
end