Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000611 RID: 1553
Public Class FlowerLevelPollenProjectile
	Inherits BasicProjectile

	' Token: 0x06001F51 RID: 8017 RVA: 0x0011FC30 File Offset: 0x0011E030
	Public Sub InitPollen(speed As Single, strength As Single, type As Integer, target As Transform)
		Me.pct = 0F
		Me.time = 0.7795515F
		Me.manual = True
		Me.speed = -speed
		Me.waveStrength = strength
		Me.target = target
		Me.Speed = 0F
		Me.move = False
		If type = 1 Then
			Me.SetParryable(True)
			MyBase.animator.Play("Pink_Idle")
		End If
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.spawn_petals_cr(type))
	End Sub

	' Token: 0x06001F52 RID: 8018 RVA: 0x0011FCBC File Offset: 0x0011E0BC
	Public Sub StartMoving()
		Me.manual = False
		Me.move = True
		Me.Speed = Me.speed
		Me.initPosY = MyBase.transform.position.y
	End Sub

	' Token: 0x06001F53 RID: 8019 RVA: 0x0011FCFC File Offset: 0x0011E0FC
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.rotate_cr())
	End Sub

	' Token: 0x06001F54 RID: 8020 RVA: 0x0011FD14 File Offset: 0x0011E114
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			If CupheadTime.GlobalSpeed <> 0F Then
				If Not Me.manual Then
					Dim position As Vector3 = MyBase.transform.position
					position.y = Me.initPosY + Mathf.Sin(Me.time * 6F) * (Me.waveStrength * Me.pct) * CupheadTime.GlobalSpeed
					MyBase.transform.position = position
					If Me.pct < 1F Then
						Me.pct += CupheadTime.FixedDelta * 2F
					Else
						Me.pct = 1F
					End If
				Else
					MyBase.transform.position = Me.target.position
					Me.Speed = 0F
				End If
				Me.time += CupheadTime.FixedDelta
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06001F55 RID: 8021 RVA: 0x0011FD30 File Offset: 0x0011E130
	Private Iterator Function rotate_cr() As IEnumerator
		Dim val As Single = If((Not Rand.Bool()), 420F, (-420F))
		Dim frameTime As Single = 0F
		While True
			frameTime += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				frameTime -= 0.041666668F
				Me.sprite.transform.Rotate(0F, 0F, val * CupheadTime.Delta)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001F56 RID: 8022 RVA: 0x0011FD4C File Offset: 0x0011E14C
	Private Iterator Function spawn_petals_cr(type As Integer) As IEnumerator
		While True
			If type = 1 Then
				Me.petalPink.Create(MyBase.transform.position)
			Else
				Me.petal.Create(MyBase.transform.position)
			End If
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.2F, 1F))
		End While
		Return
	End Function

	' Token: 0x06001F57 RID: 8023 RVA: 0x0011FD70 File Offset: 0x0011E170
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Dim instance As CupheadRenderer = CupheadRenderer.Instance
			instance.TouchFuzzy(15F, 8F, 1.2F)
			MyBase.GetComponent(Of AudioWarble)().HandleWarble()
		End If
	End Sub

	' Token: 0x06001F58 RID: 8024 RVA: 0x0011FDB2 File Offset: 0x0011E1B2
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001F59 RID: 8025 RVA: 0x0011FDC5 File Offset: 0x0011E1C5
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.petal = Nothing
		Me.petalPink = Nothing
	End Sub

	' Token: 0x040027EB RID: 10219
	Private Const ROTATE_FRAME_TIME As Single = 0.041666668F

	' Token: 0x040027EC RID: 10220
	<SerializeField()>
	Private sprite As SpriteRenderer

	' Token: 0x040027ED RID: 10221
	<SerializeField()>
	Private petalPink As FlowerLevelPollenPetal

	' Token: 0x040027EE RID: 10222
	<SerializeField()>
	Private petal As FlowerLevelPollenPetal

	' Token: 0x040027EF RID: 10223
	Private manual As Boolean

	' Token: 0x040027F0 RID: 10224
	Private time As Single

	' Token: 0x040027F1 RID: 10225
	Private speed As Single

	' Token: 0x040027F2 RID: 10226
	Private waveStrength As Single

	' Token: 0x040027F3 RID: 10227
	Private initPosY As Single

	' Token: 0x040027F4 RID: 10228
	Private target As Transform

	' Token: 0x040027F5 RID: 10229
	Private pct As Single
End Class
