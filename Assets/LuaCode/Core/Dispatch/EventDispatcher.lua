

---@class Listener
--[[
-- added by fsy @ 2017-11-28
-- 消息系统
-- 使用范例：
-- 1、模块实例销毁时，要自动移除消息监听，不移除的话不能自动清理监听
-- 2、换句话说：广播发出，回调一定会被调用，但回调参数中的实例对象，可能已经被销毁，所以回调函数一定要注意判空
-- 3、先添加的监听会先被通知,后添加的监听后通知(调用顺序可以保证不会错过某些触发时机)
-- 4、支持在事件类型的某个监听中,移除任何一个监听,包括移除自身、移除下一个将要被调用的等
--]]

local EventDispatcher = class("EventDispatcher")

local _removeNode = nil
local _createNode = nil

function EventDispatcher:OnCreate()
	self.listenerID_ = 0
	self.listeners_ = {}
	self.listenerKeys_ = {}
end

function EventDispatcher:OnDestroy()
	self.listenerID_ = 0
	self.listeners_ = nil
	self.listenerKeys_ = nil
end

function EventDispatcher:Add(lisType, listener)
	self.listenerID_ = self.listenerID_ + 1
	local appenedNode = nil
	if not self.listeners_[lisType] then
		local headNode = _createNode()
		self.listeners_[lisType] = headNode

		appenedNode = _createNode(self.listeners_[lisType], listener, self.listenerID_, lisType)
		headNode.next_ = appenedNode
		headNode.tail_ = appenedNode
	else
		local tail = self.listeners_[lisType].tail_
		appenedNode = _createNode(self.listeners_[lisType].tail_, listener, self.listenerID_, lisType)
		tail.next_ = appenedNode
		self.listeners_[lisType].tail_ = appenedNode
	end
	self.listenerKeys_[self.listenerID_] = appenedNode

	return self.listenerID_
end

function EventDispatcher:Remove(listenerID)
	local lsNode = self.listenerKeys_[listenerID]
	if lsNode then
		_removeNode(self, lsNode)
		self.listenerKeys_[listenerID] = nil
	end
end

function EventDispatcher:Clear(self)
	self:__init()
end

function EventDispatcher:Send(e_type, ...)
	local lsNode = self.listeners_[e_type]
	if lsNode then
		while lsNode and lsNode.next_ do
			local handler= lsNode.next_
			handler.listener_(...)
			if handler == lsNode.next_ then
				lsNode = lsNode.next_
			end
		end
	end
end


--[[--
描述: 派发消息，遍历普通监听列表，职责链方式，找到一个能处理的函数，则返回
@param  type, 事件类型
@param  ... , 事件参数
]]
function EventDispatcher:SendWithInterrupt(e_type, ...)
	local lsNode = self.listeners_[e_type]
	if lsNode then
		while lsNode and lsNode.next_ do
			local handler= lsNode.next_

			if handler.listener_(...) then
				return
			end

			if handler == lsNode.next_ then
				lsNode = lsNode.next_
			end
		end
	end
end

-------------------------- 私有函数实现 --------------------------
function _removeNode(self, listeneNode)
	if listeneNode then
		local eventType = listeneNode.lsType_
		if listeneNode == self.listeners_[eventType].tail_ then
			self.listeners_[eventType].tail_ = listeneNode.front_
			listeneNode.front_.next_ = nil
		else
			listeneNode.front_.next_ = listeneNode.next_
			listeneNode.next_.front_ = listeneNode.front_
		end
		if not self.listeners_[eventType].tail_.front_ then
			self.listeners_[eventType] = nil
		end
	end
end

function _createNode(front, listener, listenerId, lsType)
	return {
		front_ = front,
		listener_ = listener,
		listenerId_ = listenerId,
		lsType_ = lsType
	}
end

return EventDispatcher