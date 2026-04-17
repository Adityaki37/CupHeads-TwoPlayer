Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000818 RID: 2072
Public Class TrainLevelForegroundDynamicizer
	Inherits AbstractPausableComponent

	' Token: 0x0600300B RID: 12299 RVA: 0x001C631E File Offset: 0x001C471E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.ResetPositions()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x0600300C RID: 12300 RVA: 0x001C633C File Offset: 0x001C473C
	Private Sub ResetPositions()
		For Each spriteRenderer As SpriteRenderer In Me.sprites
			spriteRenderer.transform.SetLocalPosition(New Single?(-1280F), New Single?(0F), New Single?(0F))
		Next
	End Sub

	' Token: 0x0600300D RID: 12301 RVA: 0x001C6394 File Offset: 0x001C4794
	Private Iterator Function loop_cr() As IEnumerator
		While True
			Me.ResetPositions()
			Dim t As Single = 0F
			Dim trans As Transform = Me.sprites(Global.UnityEngine.Random.Range(0, Me.sprites.Length)).transform
			trans.SetScale(New Single?(CSng(If((Global.UnityEngine.Random.value <= 0.5F), (-1), 1))), New Single?(1F), New Single?(1F))
			While t < 1.3F
				Dim x As Single = Mathf.Lerp(1280F, -1280F, t / 1.3F)
				trans.SetLocalPosition(New Single?(x), New Single?(0F), New Single?(0F))
				t += CupheadTime.Delta
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(4F, 4F))
		End While
		Return
	End Function

	' Token: 0x040038DF RID: 14559
	Public Const X_OUT As Single = -1280F

	' Token: 0x040038E0 RID: 14560
	Public Const X_IN As Single = 1280F

	' Token: 0x040038E1 RID: 14561
	Public Const DELAY_MIN As Single = 1F

	' Token: 0x040038E2 RID: 14562
	Public Const DELAY_MAX As Single = 4F

	' Token: 0x040038E3 RID: 14563
	Public Const TIME As Single = 1.3F

	' Token: 0x040038E4 RID: 14564
	<SerializeField()>
	Private sprites As SpriteRenderer()
End Class
