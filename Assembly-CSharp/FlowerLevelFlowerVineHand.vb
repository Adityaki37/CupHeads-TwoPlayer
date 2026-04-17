Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200060B RID: 1547
Public Class FlowerLevelFlowerVineHand
	Inherits AbstractCollidableObject

	' Token: 0x06001F18 RID: 7960 RVA: 0x0011DF8C File Offset: 0x0011C38C
	Public Sub OnVineHandSpawn(firstHold As Single, secondHold As Single, attackPosOne As Integer, Optional attackPosTwo As Integer = 0)
		Me.holdCount = 0
		Dim num As Integer = attackPosOne
		For i As Integer = 0 To 2 - 1
			If i = 1 Then
				If attackPosTwo = 0 Then
					Exit For
				End If
				num = attackPosTwo
			End If
			Select Case num
				Case 1
					Me.spawnPosition = New Vector3(CSng(Me.platformOneXPosition), CSng(Me.vineHandSpawnYPosition), 0F)
				Case 2
					Me.spawnPosition = New Vector3(CSng(Me.platformTwoXPosition), CSng(Me.vineHandSpawnYPosition), 0F)
				Case 3
					Me.spawnPosition = New Vector3(CSng(Me.platformThreeXPosition), CSng(Me.vineHandSpawnYPosition), 0F)
			End Select
			Me.Create(firstHold, secondHold)
		Next
	End Sub

	' Token: 0x06001F19 RID: 7961 RVA: 0x0011E059 File Offset: 0x0011C459
	Public Sub InitVineHand(first As Single, second As Single)
		Me.firstHoldDelay = first
		Me.secondHoldDelay = second
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001F1A RID: 7962 RVA: 0x0011E074 File Offset: 0x0011C474
	Private Sub Create(first As Single, second As Single)
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject, Me.spawnPosition, Quaternion.identity)
		gameObject.GetComponent(Of FlowerLevelFlowerVineHand)().InitVineHand(first, second)
	End Sub

	' Token: 0x06001F1B RID: 7963 RVA: 0x0011E0A5 File Offset: 0x0011C4A5
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001F1C RID: 7964 RVA: 0x0011E0BD File Offset: 0x0011C4BD
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001F1D RID: 7965 RVA: 0x0011E0E8 File Offset: 0x0011C4E8
	Private Iterator Function holdDelay(delay As Single) As IEnumerator
		MyBase.animator.SetBool("OnHold", True)
		If delay <> 0F Then
			Yield CupheadTime.WaitForSeconds(Me, delay)
		End If
		MyBase.animator.SetBool("OnHold", False)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001F1E RID: 7966 RVA: 0x0011E10C File Offset: 0x0011C50C
	Private Sub OnHold()
		If Me.holdCount = 0 Then
			MyBase.StartCoroutine(Me.holdDelay(Me.firstHoldDelay))
			Me.holdCount += 1
		Else
			MyBase.StartCoroutine(Me.holdDelay(Me.secondHoldDelay))
		End If
	End Sub

	' Token: 0x06001F1F RID: 7967 RVA: 0x0011E15D File Offset: 0x0011C55D
	Private Sub OnRetracted()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001F20 RID: 7968 RVA: 0x0011E170 File Offset: 0x0011C570
	Private Sub ContinueBackAnimation()
		MyBase.animator.SetTrigger("ContinueBackAnimation")
	End Sub

	' Token: 0x06001F21 RID: 7969 RVA: 0x0011E182 File Offset: 0x0011C582
	Private Sub SoundVineHandGrow()
		AudioManager.Play("flower_vinehand_grow")
		Me.emitAudioFromObject.Add("flower_vinehand_grow")
	End Sub

	' Token: 0x06001F22 RID: 7970 RVA: 0x0011E19E File Offset: 0x0011C59E
	Private Sub SoundVineHandGrowContinue()
		AudioManager.Play("flower_vinehand_grow_continue")
		Me.emitAudioFromObject.Add("flower_vinehand_grow_continue")
	End Sub

	' Token: 0x06001F23 RID: 7971 RVA: 0x0011E1BA File Offset: 0x0011C5BA
	Private Sub SoundVineHandGrowRetract()
		AudioManager.Play("flower_vinehand_grow_retract")
		Me.emitAudioFromObject.Add("flower_vinehand_grow_retract")
	End Sub

	' Token: 0x040027B6 RID: 10166
	<SerializeField()>
	Private platformOneXPosition As Integer

	' Token: 0x040027B7 RID: 10167
	<SerializeField()>
	Private platformTwoXPosition As Integer

	' Token: 0x040027B8 RID: 10168
	<SerializeField()>
	Private platformThreeXPosition As Integer

	' Token: 0x040027B9 RID: 10169
	<Space(10F)>
	<SerializeField()>
	Private vineHandSpawnYPosition As Integer

	' Token: 0x040027BA RID: 10170
	Private holdCount As Integer

	' Token: 0x040027BB RID: 10171
	Private firstHoldDelay As Single

	' Token: 0x040027BC RID: 10172
	Private secondHoldDelay As Single

	' Token: 0x040027BD RID: 10173
	Private spawnPosition As Vector3

	' Token: 0x040027BE RID: 10174
	Private damageDealer As DamageDealer
End Class
