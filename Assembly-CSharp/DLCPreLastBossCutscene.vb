Imports System
Imports UnityEngine

' Token: 0x02000403 RID: 1027
Public Class DLCPreLastBossCutscene
	Inherits DLCGenericCutscene

	' Token: 0x06000E45 RID: 3653 RVA: 0x00092414 File Offset: 0x00090814
	Protected Overrides Sub Start()
		MyBase.Start()
		If Me.trappedChar = DLCGenericCutscene.TrappedChar.None Then
			Me.trappedChar = MyBase.DetectCharacter()
		End If
		Dim trappedChar As DLCGenericCutscene.TrappedChar = Me.trappedChar
		If trappedChar <> DLCGenericCutscene.TrappedChar.Chalice Then
			If trappedChar <> DLCGenericCutscene.TrappedChar.Mugman Then
				If trappedChar = DLCGenericCutscene.TrappedChar.Cuphead Then
					Me.trappedChalice(0).SetActive(False)
					Me.trappedChalice(1).SetActive(False)
					Me.trappedMugman(0).SetActive(False)
					Me.trappedMugman(1).SetActive(False)
					Me.text(5) = Me.altText
					Me.text(6) = Me.altTextTrappedCharCuphead
				End If
			Else
				Me.trappedChalice(0).SetActive(False)
				Me.trappedChalice(1).SetActive(False)
				Me.trappedCuphead(0).SetActive(False)
				Me.trappedCuphead(1).SetActive(False)
				Me.text(5) = Me.altText
				Me.text(6) = Me.altTextTrappedCharMugman
			End If
		Else
			Me.trappedMugman(0).SetActive(False)
			Me.trappedMugman(1).SetActive(False)
			Me.trappedCuphead(0).SetActive(False)
			Me.trappedCuphead(1).SetActive(False)
		End If
	End Sub

	' Token: 0x06000E46 RID: 3654 RVA: 0x0009254E File Offset: 0x0009094E
	Protected Overrides Sub OnCutsceneOver()
		SceneLoader.LoadLevel(Levels.Saltbaker, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x0400178D RID: 6029
	<SerializeField()>
	Private trappedChar As DLCGenericCutscene.TrappedChar

	' Token: 0x0400178E RID: 6030
	<SerializeField()>
	Private trappedChalice As GameObject()

	' Token: 0x0400178F RID: 6031
	<SerializeField()>
	Private trappedMugman As GameObject()

	' Token: 0x04001790 RID: 6032
	<SerializeField()>
	Private trappedCuphead As GameObject()

	' Token: 0x04001791 RID: 6033
	<SerializeField()>
	Private altText As GameObject

	' Token: 0x04001792 RID: 6034
	<SerializeField()>
	Private altTextTrappedCharCuphead As GameObject

	' Token: 0x04001793 RID: 6035
	<SerializeField()>
	Private altTextTrappedCharMugman As GameObject
End Class
