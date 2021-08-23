---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-20 19:03:44
---------------------------------------------------------------------

-- To edit this template in: Data/Config/Template.lua
-- To disable this template, check off menuitem: Options-Enable Template File

---@class Game
local Game = {}
local focusGameName = "hall"

local gameStack = {}
gameStack[1] = "hall"

local _readConfig
local _changeScene
local _clearEnv

function Game.Open(gameName)
	table.insert(gameStack, gameName)
	focusGameName = gameName
	Dispatch.RequestGameDispatcher(gameName)
	_readConfig(gameName)
	_changeScene(gameName)
end

function Game.Back()
	if #gameStack == 1 then
		-- todo 大厅返回只能退出游戏
		return
	end
	local game = table.remove(gameStack, #gameStack)
	_clearEnv(game)
	local cur = gameStack[#gameStack]
	_changeScene(cur)
	focusGameName = cur
end

function Game.Current()
	return focusGameName
end

function _readConfig()
	
end

function _clearEnv(gameName)
	Dispatch.CloseDispatcher(gameName)
	local contains = string.contains
	local split = string.split
	local dirty = false
	for path,_ in pairs(package.preload) do
		if contains(path, gameName) then
			package.preload[path] = nil
			local files = split(path, '.')
			local file = files[#files]
			clearclass(file)
			dirty = true
		end
	end
	
	if dirty then
		collectgarbage("collect")
	end
end

function _changeScene(gameName)
	if gameName == "hall" then
		-- TODO: 只激活大厅场景
	end
	-- 否则卸载当前场景进入下一个场景
end

return Game