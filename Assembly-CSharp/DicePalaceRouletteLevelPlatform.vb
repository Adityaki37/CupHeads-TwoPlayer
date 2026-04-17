Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005E4 RID: 1508
Public Class DicePalaceRouletteLevelPlatform
	Inherits ParrySwitch

	' Token: 0x17000372 RID: 882
	' (get) Token: 0x06001DE1 RID: 7649 RVA: 0x00112D52 File Offset: 0x00111152
	' (set) Token: 0x06001DE2 RID: 7650 RVA: 0x00112D7A File Offset: 0x0011117A
	Public Property enabled As Boolean
		Get
			Return MyBase.GetComponent(Of CircleCollider2D)().enabled AndAlso Not Me.platform.GetComponent(Of BoxCollider2D)().enabled
		End Get
		Set(value As Boolean)
			MyBase.GetComponent(Of CircleCollider2D)().enabled = value
			Me.platform.GetComponent(Of BoxCollider2D)().enabled = Not value
		End Set
	End Property

	' Token: 0x06001DE3 RID: 7651 RVA: 0x00112D9C File Offset: 0x0011119C
	Private Sub Start()
		Me.maxCounter = Global.UnityEngine.Random.Range(1, 4)
		MyBase.animator.SetBool("isOffset", Me.isOffset)
		Me.enabled = True
		MyBase.StartCoroutine(Me.sparkles_cr())
	End Sub

	' Token: 0x06001DE4 RID: 7652 RVA: 0x00112DD5 File Offset: 0x001111D5
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		Me.enabled = False
		MyBase.animator.SetBool("isFlipped", Not Me.enabled)
		MyBase.StartCoroutine(Me.timer_cr())
	End Sub

	' Token: 0x06001DE5 RID: 7653 RVA: 0x00112E04 File Offset: 0x00111204
	Private Iterator Function timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.platformOpenDuration)
		Me.enabled = True
		MyBase.animator.SetBool("isFlipped", Not Me.enabled)
		Return
	End Function

	' Token: 0x06001DE6 RID: 7654 RVA: 0x00112E1F File Offset: 0x0011121F
	Public Sub Init(properties As LevelProperties.DicePalaceRoulette.Platform)
		Me.properties = properties
	End Sub

	' Token: 0x06001DE7 RID: 7655 RVA: 0x00112E28 File Offset: 0x00111228
	Private Sub CheckSheen()
		If Me.counter < Me.maxCounter Then
			Me.sheen.enabled = False
			Me.counter += 1
		Else
			Me.sheen.enabled = True
			Me.maxCounter = Global.UnityEngine.Random.Range(1, 4)
			Me.counter = 0
		End If
	End Sub

	' Token: 0x06001DE8 RID: 7656 RVA: 0x00112E88 File Offset: 0x00111288
	Private Iterator Function sparkles_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.25F, 1F))
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_1") OrElse MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_2") Then
				MyBase.animator.SetTrigger("onSparkle")
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040026AE RID: 9902
	<SerializeField()>
	Private sheen As SpriteRenderer

	' Token: 0x040026AF RID: 9903
	<SerializeField()>
	Private isOffset As Boolean

	' Token: 0x040026B0 RID: 9904
	<SerializeField()>
	Private platform As GameObject

	' Token: 0x040026B1 RID: 9905
	Private properties As LevelProperties.DicePalaceRoulette.Platform

	' Token: 0x040026B2 RID: 9906
	Private pink As Color

	' Token: 0x040026B3 RID: 9907
	Private maxCounter As Integer

	' Token: 0x040026B4 RID: 9908
	Private counter As Integer
End Class
