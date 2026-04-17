Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000513 RID: 1299
Public Class BeeLevelGruntBriefcase
	Inherits AbstractProjectile

	' Token: 0x06001725 RID: 5925 RVA: 0x000CFCDC File Offset: 0x000CE0DC
	Public Function Create(xScale As Integer, pos As Vector2) As BeeLevelGruntBriefcase
		Dim beeLevelGruntBriefcase As BeeLevelGruntBriefcase = Global.UnityEngine.[Object].Instantiate(Of BeeLevelGruntBriefcase)(Me)
		beeLevelGruntBriefcase.transform.position = pos
		beeLevelGruntBriefcase.transform.SetScale(New Single?(CSng(xScale)), New Single?(1F), New Single?(1F))
		beeLevelGruntBriefcase.CollisionDeath.OnlyPlayer()
		beeLevelGruntBriefcase.DamagesType.OnlyPlayer()
		Return beeLevelGruntBriefcase
	End Function

	' Token: 0x06001726 RID: 5926 RVA: 0x000CFD3F File Offset: 0x000CE13F
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.x_cr())
		MyBase.StartCoroutine(Me.y_cr())
	End Sub

	' Token: 0x06001727 RID: 5927 RVA: 0x000CFD61 File Offset: 0x000CE161
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001728 RID: 5928 RVA: 0x000CFD80 File Offset: 0x000CE180
	Private Iterator Function x_cr() As IEnumerator
		While True
			If MyBase.transform.position.x < -1280F OrElse MyBase.transform.position.x > 1280F Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			MyBase.transform.AddPosition(200F * CupheadTime.Delta * -MyBase.transform.localScale.x, 0F, 0F)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001729 RID: 5929 RVA: 0x000CFD9C File Offset: 0x000CE19C
	Private Iterator Function y_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.5F
		While t < time
			Dim val As Single = t / time
			Dim speed As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 1500F, 0F, val) * CupheadTime.Delta
			MyBase.transform.AddPosition(0F, speed, 0F)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		t = 0F
		While t < time
			Dim val2 As Single = t / time
			Dim speed2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0F, -1500F, val2) * CupheadTime.Delta
			MyBase.transform.AddPosition(0F, speed2, 0F)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		While True
			If MyBase.transform.position.y < -720F Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			MyBase.transform.AddPosition(0F, -1500F * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002067 RID: 8295
	Private Const Y_SPEED As Single = 1500F

	' Token: 0x04002068 RID: 8296
	Private Const X_SPEED As Single = 200F

	' Token: 0x04002069 RID: 8297
	Private Const TIME As Single = 0.5F
End Class
