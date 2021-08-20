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
local _loadGame
local _clearEnv

function Game.Open(gameName)
	table.insert(gameStack, gameName)
	focusGameName = gameName
	_readConfig(gameName)
	_loadGame(gameName)
end

function Game.Back()
	if #gameStack == 1 then
		-- todo 大厅返回只能退出游戏
		return
	end
	local game = table.remove(gameStack, #gameStack)
	_clearEnv(game)
	focusGameName = game
end


function Game.Current()
	return focusGameName
end

function _readConfig()
	
end

function _clearEnv()
	
end

function _loadGame()
	
end


return Game