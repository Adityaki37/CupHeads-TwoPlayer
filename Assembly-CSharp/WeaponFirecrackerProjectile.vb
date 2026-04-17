Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A7B RID: 2683
Public Class WeaponFirecrackerProjectile
	Inherits BasicProjectile

	' Token: 0x0600401D RID: 16413 RVA: 0x0022F4A0 File Offset: 0x0022D8A0
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.parent IsNot Nothing AndAlso Not Me.brokeOffFromParent AndAlso Me.parent.transform.localScale.x <> Me.player.motor.LookDirection.x Then
			MyBase.transform.SetParent(Nothing, True)
			Me.brokeOffFromParent = True
		End If
	End Sub

	' Token: 0x0600401E RID: 16414 RVA: 0x0022F51D File Offset: 0x0022D91D
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		Me.hitEnemy = True
		Me.animator.Play("Hit")
		Me.collider.enabled = False
	End Sub

	' Token: 0x0600401F RID: 16415 RVA: 0x0022F54C File Offset: 0x0022D94C
	Public Sub SetupFirecracker(parent As Transform, player As LevelPlayerController, isTypeB As Boolean)
		MyBase.transform.SetParent(parent, True)
		Me.parent = parent
		Me.player = player
		Me.distanceTraveled = 0F
		If isTypeB Then
			MyBase.StartCoroutine(Me.bullet_life_B_cr())
		Else
			MyBase.StartCoroutine(Me.bullet_life_cr())
		End If
	End Sub

	' Token: 0x06004020 RID: 16416 RVA: 0x0022F5A4 File Offset: 0x0022D9A4
	Public Sub StillBullet()
		Me.move = False
		MyBase.StartCoroutine(Me.bullet_slice_life_cr())
	End Sub

	' Token: 0x06004021 RID: 16417 RVA: 0x0022F5BC File Offset: 0x0022D9BC
	Private Iterator Function bullet_life_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.bulletLife)
		Me.move = False
		Me.collider.enabled = True
		MyBase.transform.SetScale(New Single?(Me.explosionSize), New Single?(Me.explosionSize), Nothing)
		Yield CupheadTime.WaitForSeconds(Me, Me.explosionDuration)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06004022 RID: 16418 RVA: 0x0022F5D8 File Offset: 0x0022D9D8
	Private Iterator Function bullet_life_B_cr() As IEnumerator
		Dim explodeDistance As Single = Me.bulletLife * Me.Speed
		While Me.distanceTraveled < explodeDistance
			Yield Nothing
		End While
		Me.move = False
		Dim slice As WeaponFirecrackerProjectile = Global.UnityEngine.[Object].Instantiate(Of WeaponFirecrackerProjectile)(Me.projectile)
		Dim dir As Vector3 = MathUtils.AngleToDirection(Me.explosionAngle)
		slice.transform.position = MyBase.transform.position + dir * Me.explosionRadiusSize
		slice.collider.enabled = True
		slice.collider.transform.SetScale(New Single?(Me.explosionSize), New Single?(Me.explosionSize), Nothing)
		slice.DamageRate = Me.DamageRate
		slice.StillBullet()
		slice.gameObject.name = "FirecrackerExplosion"
		slice.transform.eulerAngles = New Vector3(0F, 0F, Me.explosionAngle)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x06004023 RID: 16419 RVA: 0x0022F5F4 File Offset: 0x0022D9F4
	Private Iterator Function bullet_slice_life_cr() As IEnumerator
		Me.hitEnemy = False
		Me.animator.Play("Die")
		Yield CupheadTime.WaitForSeconds(Me, Me.explosionDuration)
		If Me.hitEnemy Then
			Dim sprite As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
			While sprite.enabled
				Yield Nothing
			End While
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x06004024 RID: 16420 RVA: 0x0022F610 File Offset: 0x0022DA10
	Protected Overrides Sub Move()
		Me.moveVector = MyBase.transform.right * Me.Speed * CupheadTime.FixedDelta - New Vector3(0F, Me._accumulativeGravity * CupheadTime.FixedDelta, 0F)
		MyBase.transform.position += Me.moveVector
		Me.distanceTraveled += Me.moveVector.magnitude
	End Sub

	' Token: 0x040046E0 RID: 18144
	<SerializeField()>
	Private projectile As WeaponFirecrackerProjectile

	' Token: 0x040046E1 RID: 18145
	Public bulletLife As Single

	' Token: 0x040046E2 RID: 18146
	Public explosionSize As Single

	' Token: 0x040046E3 RID: 18147
	Public explosionDuration As Single

	' Token: 0x040046E4 RID: 18148
	Public explosionRadiusSize As Single

	' Token: 0x040046E5 RID: 18149
	Public explosionAngle As Single

	' Token: 0x040046E6 RID: 18150
	Private parent As Transform

	' Token: 0x040046E7 RID: 18151
	Private player As LevelPlayerController

	' Token: 0x040046E8 RID: 18152
	Private parentScaleX As Single

	' Token: 0x040046E9 RID: 18153
	Private brokeOffFromParent As Boolean

	' Token: 0x040046EA RID: 18154
	Private moveVector As Vector3

	' Token: 0x040046EB RID: 18155
	Private distanceTraveled As Single

	' Token: 0x040046EC RID: 18156
	Public collider As Collider2D

	' Token: 0x040046ED RID: 18157
	Public animator As Animator

	' Token: 0x040046EE RID: 18158
	Private hitEnemy As Boolean
End Class
