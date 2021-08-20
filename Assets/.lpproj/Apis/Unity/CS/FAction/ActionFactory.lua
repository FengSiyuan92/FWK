---@class CS.FAction.ActionFactory : CS.System.Object
CS.FAction.ActionFactory = {}

---@field public CS.FAction.ActionFactory.syncActionPool : CS.ReuseObjectPool
CS.FAction.ActionFactory.syncActionPool = nil

---@field public CS.FAction.ActionFactory.repeatActionPool : CS.ReuseObjectPool
CS.FAction.ActionFactory.repeatActionPool = nil

---@field public CS.FAction.ActionFactory.sequenceActionPool : CS.ReuseObjectPool
CS.FAction.ActionFactory.sequenceActionPool = nil

---@field public CS.FAction.ActionFactory.waitFastActionPool : CS.ReuseObjectPool
CS.FAction.ActionFactory.waitFastActionPool = nil

---@field public CS.FAction.ActionFactory.callFuncActionPool : CS.ReuseObjectPool
CS.FAction.ActionFactory.callFuncActionPool = nil

---@field public CS.FAction.ActionFactory.delayActionPool : CS.ReuseObjectPool
CS.FAction.ActionFactory.delayActionPool = nil

---@field public CS.FAction.ActionFactory.waitFrameActionPool : CS.ReuseObjectPool
CS.FAction.ActionFactory.waitFrameActionPool = nil

---@field public CS.FAction.ActionFactory.transformToActionPool : CS.ReuseObjectPool
CS.FAction.ActionFactory.transformToActionPool = nil

---@field public CS.FAction.ActionFactory.graphicToAction : CS.ReuseObjectPool
CS.FAction.ActionFactory.graphicToAction = nil

---@return CS.FAction.ActionFactory
function CS.FAction.ActionFactory()
end

---@param action : CS.FAction.FAction
function CS.FAction.ActionFactory.ReturnActionInstance(action)
end

function CS.FAction.ActionFactory.Preload()
end