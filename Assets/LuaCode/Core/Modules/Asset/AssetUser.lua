---------------------------------------------------------------------
-- FWK (C) CompanyName, All Rights Reserved
-- Created by: AuthorName
-- Date: 2021-08-23 20:43:11
---------------------------------------------------------------------

-- 资源调度器,所有luaObject通过调用该接口获取到的所有资源
-- 在LuaObject生命周期结束时(Destroy)都会自动回收并释放
-- 也可以手动的调用Recycle函数,在任意时间内释放某个资源
-- Get是同步接口,Set是异步接口



---@class Asset
local AssetUser = {}
local meta = {__index = AssetUser}
local Asset = CS.AssetRuntime.Asset
local AssetManager = CS.AssetManager

local _createSafeCallback

function AssetUser.Create(owner, mod)
	mod.owner_ = owner
	mod.assets_ = {}
	mod.request_ = {}
	setmetatable(mod, meta)
end

function AssetUser.Destroy(mod)
	if mod.request_ then
		for k, request in pairs(mod.request_) do
			request:OnInterrupt()
		end
		mod.request_ = nil
	end

	if mod.assets_ then
		for assetName, cache in pairs(mod.assets_) do
			Asset.ReturnAsset(assetName)
		end
		mod.assets_ = nil
	end
end

-- get同步接口
function AssetUser:Get(assetName)
	return self.assets_[assetName]
end


-- 同步getsprite
function AssetUser:GetSprite(assetname)
	local atlasName = AssetManager.GetAtlasName(assetname)
	if not atlasName or atlasName == "" then
		error("没有在图集映射中找到名为'" ..assetname .."'的sprite")
	end
	local cache = self.assets_[atlasName]
	if cache then
		local sprite = cache:GetSprite(assetname)
		return sprite
	end
end

-- 异步设置sprite
function AssetUser:LoadSprite(assetname, callback)
	local atlasName = AssetManager.GetAtlasName(assetname)
	if not atlasName or atlasName == "" then
		error("没有在图集映射中找到名为'" ..assetname .."'的sprite")
	end
	-- 已经有缓存的资源,直接返回
	local cache = self.assets_[atlasName]
	if cache then
		local sprite = cache:GetSprite(assetname)
		callback(sprite)
		return
	end

	if not self.loadingAtlas_ then
		self.loadingAtlas_ = {}
	end	
	
	local waitLoading = self.loadingAtlas_[atlasName]
	if waitLoading then
		waitLoading[#waitLoading + 1] = {assetname = assetname, callback = callback}
		return
	end
	
	self.loadingAtlas_[atlasName] = {{assetname = assetname, callback = callback}}
	
	local request
	request = Asset.GetAssetAsync(atlasName, function(obj)
			self.assets_[atlasName] = obj
			for k, info in pairs(self.loadingAtlas_[atlasName]) do
				local sprite = obj:GetSprite(info.assetname)
				info.callback(sprite)
			end
			self.loadingAtlas_[atlasName] = nil
			self.request_[atlasName] = nil
		end)
	if request then
		self.request_[atlasName] = request
	end
end


-- 异步load接口
function AssetUser:Load(assetName, callback)
	-- 已经有缓存的资源,直接返回
	local cache = self.assets_[assetName]
	if cache then
		callback(cache)
		return
	end
	local request = self.request_[assetName]
	
	if request then
		error("不要在同一个object中对同一资源同时申请加载多次")
	end
	-- 如果有request则说明进行着异步行为,需要进行缓存
	request = Asset.GetAssetAsync(assetName, function(obj)
		self.assets_[assetName] = obj
		self.request_[assetName] = nil
		callback(obj)
	end)
	
	if request then
		self.request_[assetName] = request
	end
end

---------回收资源接口-------------
function AssetUser.Recycle(assetName)
	-- 需要回收的资源是否正在加载中
	local request = self.request_[assetName]
	if request then
		request:OnInterrupt()
		return
	end
	-- 是否是缓存中的资源
	local cache = self.assets_[assetName]
	if cache then
		Asset.ReturnAsset(assetName)
		self.assets_[assetName] = nil
		return
	end
end


return AssetUser