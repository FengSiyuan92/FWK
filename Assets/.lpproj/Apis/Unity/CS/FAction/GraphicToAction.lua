---@class CS.FAction.GraphicToAction : CS.FAction.FAction
CS.FAction.GraphicToAction = {}

---@property readonly CS.FAction.GraphicToAction.ActionType : CS.FAction.ActionType
CS.FAction.GraphicToAction.ActionType = nil

---@property readwrite CS.FAction.GraphicToAction.ControlType : CS.FAction.GraphicControlType
CS.FAction.GraphicToAction.ControlType = nil

---@return CS.FAction.GraphicToAction
function CS.FAction.GraphicToAction()
end

---@param graphic : CS.UnityEngine.UI.Graphic
---@param type : CS.FAction.GraphicControlType
---@param target : CS.UnityEngine.Color
---@param duration : CS.System.Single
function CS.FAction.GraphicToAction:InitGraphicInfo(graphic, type, target, duration)
end

function CS.FAction.GraphicToAction:Replay()
end

function CS.FAction.GraphicToAction:OnStartTick()
end

function CS.FAction.GraphicToAction:Tick()
end

function CS.FAction.GraphicToAction:Clear()
end