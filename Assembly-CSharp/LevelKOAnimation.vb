Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004A6 RID: 1190
Public Class LevelKOAnimation
	Inherits AbstractLevelHUDComponent

	' Token: 0x0600136A RID: 4970 RVA: 0x000AB51F File Offset: 0x000A991F
	Public Shared Function Create(isMaus As Boolean) As LevelKOAnimation
		LevelKOAnimation.isMausoleum = isMaus
		Return Global.UnityEngine.[Object].Instantiate(Of LevelKOAnimation)(Level.Current.LevelResources.levelKO)
	End Function

	' Token: 0x0600136B RID: 4971 RVA: 0x000AB53B File Offset: 0x000A993B
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me._parentToHudCanvas = True
	End Sub

	' Token: 0x0600136C RID: 4972 RVA: 0x000AB54A File Offset: 0x000A994A
	Private Sub OnAnimComplete()
		Me.state = LevelKOAnimation.State.Complete
	End Sub

	' Token: 0x0600136D RID: 4973 RVA: 0x000AB554 File Offset: 0x000A9954
	Public Iterator Function anim_cr() As IEnumerator
		MyBase.GetComponent(Of Animator)().SetTrigger(If(LevelKOAnimation.isMausoleum, "StartMaus", "Start"))
		While Me.state = LevelKOAnimation.State.Animating
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04001C7D RID: 7293
	Private Const FRAME_DELAY As Single = 5F

	' Token: 0x04001C7E RID: 7294
	Private state As LevelKOAnimation.State

	' Token: 0x04001C7F RID: 7295
	Private Shared isMausoleum As Boolean

	' Token: 0x020004A7 RID: 1191
	Public Enum State
		' Token: 0x04001C81 RID: 7297
		Animating
		' Token: 0x04001C82 RID: 7298
		Complete
	End Enum
End Class
