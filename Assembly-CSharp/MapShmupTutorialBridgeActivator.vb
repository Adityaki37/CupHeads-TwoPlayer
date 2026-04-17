Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000960 RID: 2400
Public Class MapShmupTutorialBridgeActivator
	Inherits MonoBehaviour

	' Token: 0x06003804 RID: 14340 RVA: 0x002010F4 File Offset: 0x001FF4F4
	Private Sub Start()
		If Not PlayerData.Data.IsFlyingTutorialCompleted AndAlso Level.PreviousLevel = Levels.ShmupTutorial Then
			PlayerData.Data.IsFlyingTutorialCompleted = True
			Me.blueprintObstacle.OnConditionNotMet()
			MyBase.StartCoroutine(Me.DoTransition())
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
			PlayerData.SaveCurrentFile()
		ElseIf Not PlayerData.Data.IsFlyingTutorialCompleted Then
			Me.blueprintObstacle.OnConditionNotMet()
		Else
			Me.blueprintObstacle.OnConditionAlreadyMet()
		End If
	End Sub

	' Token: 0x06003805 RID: 14341 RVA: 0x00201188 File Offset: 0x001FF588
	Private Iterator Function DoTransition() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.DoTransitionDelay)
		Me.blueprintObstacle.DoTransition()
		Yield Nothing
		Yield Nothing
		Yield Nothing
		Yield Nothing
		Me.blueprintObstacle.OnConditionAlreadyMet()
		Return
	End Function

	' Token: 0x04003FE9 RID: 16361
	<SerializeField()>
	Private blueprintObstacle As MapLevelDependentObstacle

	' Token: 0x04003FEA RID: 16362
	<SerializeField()>
	Private DoTransitionDelay As Single

	' Token: 0x04003FEB RID: 16363
	<SerializeField()>
	Private dialoguerVariableID As Integer = 5
End Class
