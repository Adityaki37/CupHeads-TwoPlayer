Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000674 RID: 1652
Public Class FlyingGenieLevelObelisk
	Inherits AbstractCollidableObject

	' Token: 0x17000398 RID: 920
	' (get) Token: 0x060022C0 RID: 8896 RVA: 0x00146464 File Offset: 0x00144864
	' (set) Token: 0x060022C1 RID: 8897 RVA: 0x0014646C File Offset: 0x0014486C
	Public Property isOn As Boolean

	' Token: 0x17000399 RID: 921
	' (get) Token: 0x060022C2 RID: 8898 RVA: 0x00146475 File Offset: 0x00144875
	' (set) Token: 0x060022C3 RID: 8899 RVA: 0x0014647D File Offset: 0x0014487D
	Public Property isFirst As Boolean

	' Token: 0x060022C4 RID: 8900 RVA: 0x00146488 File Offset: 0x00144888
	Public Sub Init(pos As Vector2, properties As LevelProperties.FlyingGenie.Obelisk, parent As FlyingGenieLevelGenie, isFirst As Boolean)
		Me.isFirst = isFirst
		MyBase.transform.position = pos
		Me.startPosition = pos
		Me.properties = properties
		Me.parent = parent
		If isFirst Then
			AudioManager.PlayLoop("genie_pillar_main_loop")
			AudioManager.PlayLoop("genie_pillar_destructable_loop")
			Me.emitAudioFromObject.Add("genie_pillar_main_loop")
			Me.emitAudioFromObject.Add("genie_pillar_destructable_loop")
		End If
	End Sub

	' Token: 0x060022C5 RID: 8901 RVA: 0x00146503 File Offset: 0x00144903
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		If Rand.Bool() Then
			Me.baseA.SetActive(True)
		Else
			Me.baseB.SetActive(True)
		End If
	End Sub

	' Token: 0x060022C6 RID: 8902 RVA: 0x00146538 File Offset: 0x00144938
	Public Sub ActivateObelisk(genieHeadNums As String())
		Me.isOn = True
		Me.bouncerWall.enabled = True
		MyBase.transform.position = Me.startPosition
		Dim num As Single = Me.obeliskBlock.GetComponent(Of Renderer)().bounds.size.y / 1.95F
		Dim num2 As Integer = 100
		Dim num3 As Integer = 0
		Dim num4 As Integer = 5
		Dim flag As Boolean = False
		Me.obeliskBlocks = New List(Of FlyingGenieLevelObeliskBlock)()
		Me.genieHeads = New List(Of FlyingGenieLevelGenieHead)()
		Dim array As Boolean() = New Boolean(num4 - 1) {}
		Dim array2 As Integer() = New Integer(num4 - 1) {}
		For i As Integer = 0 To num4 - 1
			Dim flag2 As Boolean = False
			For Each text As String In genieHeadNums
				Parser.IntTryParse(text, num3)
				If num3 = i + 1 Then
					flag2 = True
				End If
				If genieHeadNums.Length < 2 AndAlso num3 = 2 Then
					flag = True
				End If
			Next
			array(i) = flag2
		Next
		Dim num5 As Integer = If((num3 <= 1), (num4 - 1), (num3 - 1 - 1))
		If genieHeadNums.Length > 1 Then
			If genieHeadNums(0)(0) = "2"c AndAlso genieHeadNums(1)(0) = "5"c Then
				array2(0) = 1
				array2(2) = 5
				array2(3) = 1
			ElseIf genieHeadNums(0)(0) = "1"c AndAlso genieHeadNums(1)(0) = "4"c Then
				array2(1) = 5
				array2(2) = 1
				array2(4) = 5
			ElseIf genieHeadNums(0)(0) = "1"c AndAlso genieHeadNums(1)(0) = "5"c Then
				array2(1) = 4
				array2(2) = 5
				array2(3) = 1
			End If
		Else
			For k As Integer = 0 To num4 - 1
				array2(num5) = k + 1
				num5 = (num5 + 1) Mod num4
			Next
		End If
		Dim flag3 As Boolean = Rand.Bool()
		For l As Integer = 0 To num4 - 1
			Dim vector As Vector3 = New Vector3(0F, CSng((-CSng(l))) * num - num / 1.5F, 0F)
			If array(l) Then
				Dim flyingGenieLevelGenieHead As FlyingGenieLevelGenieHead = Global.UnityEngine.[Object].Instantiate(Of FlyingGenieLevelGenieHead)(Me.genieHead)
				flyingGenieLevelGenieHead.Init(MyBase.transform.position + vector, Me.properties.obeliskGenieHP, Me.parent)
				flyingGenieLevelGenieHead.transform.parent = MyBase.transform
				flyingGenieLevelGenieHead.GetComponent(Of SpriteRenderer)().sortingOrder = num2
				flyingGenieLevelGenieHead.animator.SetBool("GoClockwise", flag3)
				Me.genieHeads.Add(flyingGenieLevelGenieHead)
			Else
				Dim flyingGenieLevelObeliskBlock As FlyingGenieLevelObeliskBlock = Global.UnityEngine.[Object].Instantiate(Of FlyingGenieLevelObeliskBlock)(Me.obeliskBlock)
				flyingGenieLevelObeliskBlock.Init(MyBase.transform.position + vector, Me.properties)
				flyingGenieLevelObeliskBlock.transform.parent = MyBase.transform
				flyingGenieLevelObeliskBlock.GetComponent(Of SpriteRenderer)().sortingOrder = num2
				flyingGenieLevelObeliskBlock.animator.SetBool("GoClockwise", flag3)
				flyingGenieLevelObeliskBlock.animator.SetInteger("PickBlock", array2(l))
				Me.obeliskBlocks.Add(flyingGenieLevelObeliskBlock)
			End If
			If Not flag Then
				flag3 = Not flag3
			End If
			num2 -= 2
		Next
		MyBase.StartCoroutine(Me.move_cr())
		If Me.properties.normalShotOn Then
			MyBase.StartCoroutine(Me.shoot_cr())
		End If
	End Sub

	' Token: 0x060022C7 RID: 8903 RVA: 0x001468A0 File Offset: 0x00144CA0
	Public Sub SetColliders(width As Single, offset As Single)
		Me.ceiling.transform.position = New Vector3(offset, Me.ceiling.transform.position.y)
		Me.ceiling.size = New Vector3(width, Me.ceiling.size.y)
		Me.ground.transform.position = New Vector3(offset, -360F)
		Me.ground.size = New Vector3(width, Me.ground.size.y)
	End Sub

	' Token: 0x060022C8 RID: 8904 RVA: 0x00146948 File Offset: 0x00144D48
	Private Iterator Function shoot_cr() As IEnumerator
		Dim anglePattern As String() = Me.properties.obeliskShotDirection.GetRandom().Split(New Char() { ","c })
		Dim pinkPattern As String() = Me.properties.obeliskPinkString.GetRandom().Split(New Char() { ","c })
		Dim angleIndex As Integer = Global.UnityEngine.Random.Range(0, anglePattern.Length)
		Dim pinkIndex As Integer = Global.UnityEngine.Random.Range(0, pinkPattern.Length)
		Dim angle As Single = 0F
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.obeliskShootDelay)
			Yield Nothing
			For Each block As FlyingGenieLevelObeliskBlock In Me.obeliskBlocks
				Parser.FloatTryParse(anglePattern(angleIndex), angle)
				If pinkPattern(pinkIndex)(0) = "P"c Then
					block.ShootPink(angle)
				Else
					block.ShootRegular(angle)
				End If
				Yield Nothing
			Next
			angleIndex = angleIndex Mod anglePattern.Length
			pinkIndex = pinkIndex Mod pinkPattern.Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060022C9 RID: 8905 RVA: 0x00146963 File Offset: 0x00144D63
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060022CA RID: 8906 RVA: 0x00146984 File Offset: 0x00144D84
	Private Iterator Function move_cr() As IEnumerator
		Me.isFirst = True
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.x > -840F
			MyBase.transform.AddPosition(-Me.properties.obeliskMovementSpeed * CupheadTime.Delta, 0F, 0F)
			Yield wait
		End While
		Me.isFirst = False
		Me.[End]()
		Yield Nothing
		Return
	End Function

	' Token: 0x060022CB RID: 8907 RVA: 0x001469A0 File Offset: 0x00144DA0
	Private Sub [End]()
		Me.isOn = False
		For Each flyingGenieLevelObeliskBlock As FlyingGenieLevelObeliskBlock In Me.obeliskBlocks
			Global.UnityEngine.[Object].Destroy(flyingGenieLevelObeliskBlock.gameObject)
		Next
		For Each flyingGenieLevelGenieHead As FlyingGenieLevelGenieHead In Me.genieHeads
			If flyingGenieLevelGenieHead IsNot Nothing Then
				Global.UnityEngine.[Object].Destroy(flyingGenieLevelGenieHead.gameObject)
			End If
		Next
		Me.obeliskBlocks.Clear()
		Me.genieHeads.Clear()
		Me.StopAllCoroutines()
		Me.bouncerWall.enabled = False
	End Sub

	' Token: 0x04002B5F RID: 11103
	<SerializeField()>
	Private baseA As GameObject

	' Token: 0x04002B60 RID: 11104
	<SerializeField()>
	Private baseB As GameObject

	' Token: 0x04002B61 RID: 11105
	<SerializeField()>
	Private bouncerWall As BoxCollider2D

	' Token: 0x04002B62 RID: 11106
	<SerializeField()>
	Private ceiling As BoxCollider2D

	' Token: 0x04002B63 RID: 11107
	<SerializeField()>
	Private ground As BoxCollider2D

	' Token: 0x04002B64 RID: 11108
	<SerializeField()>
	Private genieHead As FlyingGenieLevelGenieHead

	' Token: 0x04002B65 RID: 11109
	<SerializeField()>
	Private obeliskBlock As FlyingGenieLevelObeliskBlock

	' Token: 0x04002B66 RID: 11110
	Private obeliskBlocks As List(Of FlyingGenieLevelObeliskBlock)

	' Token: 0x04002B67 RID: 11111
	Private genieHeads As List(Of FlyingGenieLevelGenieHead)

	' Token: 0x04002B68 RID: 11112
	Private properties As LevelProperties.FlyingGenie.Obelisk

	' Token: 0x04002B69 RID: 11113
	Private parent As FlyingGenieLevelGenie

	' Token: 0x04002B6A RID: 11114
	Private damageDealer As DamageDealer

	' Token: 0x04002B6B RID: 11115
	Private startPosition As Vector3

	' Token: 0x04002B6C RID: 11116
	Private newEmitPosition As Vector3

	' Token: 0x04002B6D RID: 11117
	Private health As Single

	' Token: 0x04002B6E RID: 11118
	Private shootAngle As Single
End Class
