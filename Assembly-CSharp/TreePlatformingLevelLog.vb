Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000891 RID: 2193
Public Class TreePlatformingLevelLog
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x17000441 RID: 1089
	' (get) Token: 0x060032F9 RID: 13049 RVA: 0x001D9FE5 File Offset: 0x001D83E5
	Public ReadOnly Property CanShoot As Boolean
		Get
			Return Me.canShoot
		End Get
	End Property

	' Token: 0x17000442 RID: 1090
	' (get) Token: 0x060032FA RID: 13050 RVA: 0x001D9FED File Offset: 0x001D83ED
	Public ReadOnly Property ShootDelay As Single
		Get
			Return Me.shootDelay
		End Get
	End Property

	' Token: 0x060032FB RID: 13051 RVA: 0x001D9FF8 File Offset: 0x001D83F8
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase._damageReceiver.enabled = False
		Me.pinkPattern = Me.pinkString.Split(New Char() { ","c })
		Me.pinkIndex = Global.UnityEngine.Random.Range(0, Me.pinkPattern.Length)
	End Sub

	' Token: 0x060032FC RID: 13052 RVA: 0x001DA047 File Offset: 0x001D8447
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x060032FD RID: 13053 RVA: 0x001DA049 File Offset: 0x001D8449
	Public Sub SlideDown(belowBoundsY As Single)
		MyBase.StartCoroutine(Me.slide_cr(belowBoundsY))
	End Sub

	' Token: 0x060032FE RID: 13054 RVA: 0x001DA05C File Offset: 0x001D845C
	Private Iterator Function slide_cr(belowBoundsY As Single) As IEnumerator
		Me.isSliding = True
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.y > Me.start - belowBoundsY
			MyBase.transform.AddPosition(0F, -MyBase.Properties.MoveSpeed * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		Me.start = MyBase.transform.position.y
		Me.isSliding = False
		Yield Nothing
		Return
	End Function

	' Token: 0x060032FF RID: 13055 RVA: 0x001DA07E File Offset: 0x001D847E
	Protected Overrides Sub Die()
	End Sub

	' Token: 0x06003300 RID: 13056 RVA: 0x001DA080 File Offset: 0x001D8480
	Public Sub KillLog()
		Me.SpawnPieces()
		Me.isDying = True
		MyBase.Die()
	End Sub

	' Token: 0x06003301 RID: 13057 RVA: 0x001DA098 File Offset: 0x001D8498
	Private Sub SpawnPieces()
		AudioManager.Play("level_platform_logface_death")
		Me.emitAudioFromObject.Add("level_platform_logface_death")
		For Each spriteDeathParts As SpriteDeathParts In Me.parts
			spriteDeathParts.CreatePart(MyBase.transform.position)
		Next
	End Sub

	' Token: 0x06003302 RID: 13058 RVA: 0x001DA0F0 File Offset: 0x001D84F0
	Public Sub OnShoot()
		If Me.canShoot Then
			MyBase.animator.SetTrigger("OnShoot")
		End If
	End Sub

	' Token: 0x06003303 RID: 13059 RVA: 0x001DA110 File Offset: 0x001D8510
	Private Sub Shoot()
		Dim num As Single = MyBase.Properties.ProjectileSpeed
		If Me.facingRight Then
			num *= -1F
		End If
		Me.projectile.Create(Me.root.transform.position, 180F, num, Not Me.facingRight, Me.pinkPattern(Me.pinkIndex)(0) = "P"c)
		Me.pinkIndex = (Me.pinkIndex + 1) Mod Me.pinkPattern.Length
		Dim effect As Effect = Me.projectilePuff.Create(Me.root.transform.position)
		effect.GetComponent(Of SpriteRenderer)().flipY = Me.facingRight
	End Sub

	' Token: 0x06003304 RID: 13060 RVA: 0x001DA1C8 File Offset: 0x001D85C8
	Public Sub SetDirection(isRight As Boolean)
		Me.facingRight = isRight
		If Me.facingRight Then
			Dim localScale As Vector3 = MyBase.transform.localScale
			localScale.x *= -1F
			MyBase.transform.localScale = localScale
		End If
	End Sub

	' Token: 0x06003305 RID: 13061 RVA: 0x001DA212 File Offset: 0x001D8612
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectile = Nothing
		Me.projectilePuff = Nothing
		Me.parts = Nothing
	End Sub

	' Token: 0x04003B20 RID: 15136
	<SerializeField()>
	Private projectile As TreePlatformingLevelLogProjectile

	' Token: 0x04003B21 RID: 15137
	<SerializeField()>
	Private root As Transform

	' Token: 0x04003B22 RID: 15138
	<SerializeField()>
	Private shootDelay As Single

	' Token: 0x04003B23 RID: 15139
	<SerializeField()>
	Private parts As SpriteDeathParts()

	' Token: 0x04003B24 RID: 15140
	<SerializeField()>
	Private canShoot As Boolean

	' Token: 0x04003B25 RID: 15141
	<SerializeField()>
	Private pinkString As String

	' Token: 0x04003B26 RID: 15142
	<SerializeField()>
	Private projectilePuff As Effect

	' Token: 0x04003B27 RID: 15143
	Private facingRight As Boolean

	' Token: 0x04003B28 RID: 15144
	Private pinkPattern As String()

	' Token: 0x04003B29 RID: 15145
	Private pinkIndex As Integer

	' Token: 0x04003B2A RID: 15146
	Public isDying As Boolean

	' Token: 0x04003B2B RID: 15147
	Public isSliding As Boolean

	' Token: 0x04003B2C RID: 15148
	Public start As Single
End Class
