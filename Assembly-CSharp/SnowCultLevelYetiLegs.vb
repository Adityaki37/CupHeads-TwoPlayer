Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000801 RID: 2049
Public Class SnowCultLevelYetiLegs
	Inherits BasicDamageDealingObject

	' Token: 0x06002F5B RID: 12123 RVA: 0x001C1443 File Offset: 0x001BF843
	Private Sub Start()
		MyBase.StartCoroutine(Me.run_away_cr())
	End Sub

	' Token: 0x06002F5C RID: 12124 RVA: 0x001C1454 File Offset: 0x001BF854
	Private Iterator Function run_away_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.rend.sortingLayerName = "Player"
		Me.rend.sortingOrder = -19
		MyBase.animator.Play("Run")
		Me.SFX_SNOWCULT_YetiLegsWalkOff()
		For i As Integer = 0 To 1000 - 1
			MyBase.transform.position += Vector3.left * Mathf.Sign(MyBase.transform.localScale.x) * 1000F * CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		Next
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002F5D RID: 12125 RVA: 0x001C146F File Offset: 0x001BF86F
	Private Sub SFX_SNOWCULT_YetiLegsWalkOff()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_death_stompoffscreen")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_death_stompoffscreen")
	End Sub

	' Token: 0x0400382B RID: 14379
	Private Const RUN_SPEED As Single = 1000F

	' Token: 0x0400382C RID: 14380
	Private Const RUN_DELAY As Single = 1F

	' Token: 0x0400382D RID: 14381
	<SerializeField()>
	Private rend As SpriteRenderer
End Class
