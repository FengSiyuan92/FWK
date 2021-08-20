---@class CS.FAction.TransformToAction : CS.FAction.FAction
CS.FAction.TransformToAction = {}

---@field public CS.FAction.TransformToAction.time : CS.System.Single
CS.FAction.TransformToAction.time = nil

---@property readonly CS.FAction.TransformToAction.ActionType : CS.FAction.ActionType
CS.FAction.TransformToAction.ActionType = nil

---@property readwrite CS.FAction.TransformToAction.ControlType : CS.FAction.TransformControlType
CS.FAction.TransformToAction.ControlType = nil

---@return CS.FAction.TransformToAction
function CS.FAction.TransformToAction()
end

---@param transform : CS.UnityEngine.Transform
---@param type : CS.FAction.TransformControlType
---@param target : CS.UnityEngine.Vector3
---@param time : CS.System.Single
function CS.FAction.TransformToAction:InitTransform(transform, type, target, time)
end

function CS.FAction.TransformToAction:OnStartTick()
end

function CS.FAction.TransformToAction:Replay()
end

function CS.FAction.TransformToAction:Clear()
end

function CS.FAction.TransformToAction:Tick()
end