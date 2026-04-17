Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000659 RID: 1625
Public Class FlyingCowboyLevelRicochetDebris
	Inherits BasicUprightProjectile

	' Token: 0x060021DC RID: 8668 RVA: 0x0013B820 File Offset: 0x00139C20
	Public Overridable Function Create(position As Vector3, speed As Single, bulletSpeed As Single, bulletType As FlyingCowboyLevelRicochetDebris.BulletType, bulletParryable As Boolean) As BasicProjectile
		Dim flyingCowboyLevelRicochetDebris As FlyingCowboyLevelRicochetDebris = TryCast(Me.Create(position, MathUtils.DirectionToAngle(Vector3.down), speed), FlyingCowboyLevelRicochetDebris)
		flyingCowboyLevelRicochetDebris.bulletType = bulletType
		flyingCowboyLevelRicochetDebris.bulletSpeed = bulletSpeed
		flyingCowboyLevelRicochetDebris.bulletParryable = bulletParryable
		Return flyingCowboyLevelRicochetDebris
	End Function

	' Token: 0x060021DD RID: 8669 RVA: 0x0013B868 File Offset: 0x00139C68
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.animator.Update(0F)
		MyBase.animator.Play(0, 0, Global.UnityEngine.Random.Range(0F, 1F))
		Dim localScale As Vector3 = MyBase.transform.localScale
		localScale.x *= CSng(Rand.PosOrNeg())
		MyBase.transform.localScale = localScale
		If MyBase.animator.GetInteger(AbstractProjectile.[Variant]) = 0 Then
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(FlyingCowboyLevelRicochetDebris.AllowedRotations.GetRandom()))
		End If
		MyBase.StartCoroutine(Me.fall_cr())
		MyBase.StartCoroutine(Me.shadowScale_cr())
		MyBase.StartCoroutine(Me.shadowPosition_cr())
	End Sub

	' Token: 0x060021DE RID: 8670 RVA: 0x0013B93D File Offset: 0x00139D3D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.SFX_COWGIRL_COWGIRL_P2_SafeHitPlayer()
	End Sub

	' Token: 0x060021DF RID: 8671 RVA: 0x0013B950 File Offset: 0x00139D50
	Private Iterator Function fall_cr() As IEnumerator
		While MyBase.transform.position.y > -360F + FlyingCowboyLevelRicochetDebris.GroundOffset
			Yield Nothing
		End While
		Dim bulletType As FlyingCowboyLevelRicochetDebris.BulletType = Me.bulletType
		If bulletType <> FlyingCowboyLevelRicochetDebris.BulletType.[Nothing] Then
			If bulletType = FlyingCowboyLevelRicochetDebris.BulletType.Ricochet Then
				Me.shootRicochetProjectile()
			End If
		End If
		Me.SFX_COWGIRL_COWGIRL_P2_SafeDropImpact()
		Me.Die()
		Return
	End Function

	' Token: 0x060021E0 RID: 8672 RVA: 0x0013B96C File Offset: 0x00139D6C
	Private Iterator Function shadowScale_cr() As IEnumerator
		Me.shadowTransform.rotation = Quaternion.identity
		Dim ground As Single = -360F + FlyingCowboyLevelRicochetDebris.GroundOffset
		While MyBase.transform.position.y > ground + FlyingCowboyLevelRicochetDebris.ShadowStartOffset
			Yield Nothing
		End While
		Dim startY As Single = ground + FlyingCowboyLevelRicochetDebris.ShadowStartOffset
		Dim endY As Single = ground + FlyingCowboyLevelRicochetDebris.ShadowEndOffset
		MyBase.animator.Play("On", 1)
		Dim wait As WaitForFrameTimePersistent = New WaitForFrameTimePersistent(0.041666668F, False)
		While Not MyBase.dead
			Dim parentScale As Single = MyBase.transform.localScale.x
			Dim scale As Vector3 = Me.shadowTransform.localScale
			Dim num As Single = MathUtilities.LerpMapping(MyBase.transform.position.y, startY, endY, FlyingCowboyLevelRicochetDebris.ShadowScaleRange.min, FlyingCowboyLevelRicochetDebris.ShadowScaleRange.max, True) / parentScale
			Dim num2 As Single = num
			scale.y = num
			scale.x = num2
			Me.shadowTransform.localScale = scale
			Yield wait
		End While
		MyBase.animator.Play("Off", 1)
		Return
	End Function

	' Token: 0x060021E1 RID: 8673 RVA: 0x0013B988 File Offset: 0x00139D88
	Private Iterator Function shadowPosition_cr() As IEnumerator
		Dim ground As Single = -360F + FlyingCowboyLevelRicochetDebris.GroundOffset
		While Not MyBase.dead
			Dim position As Vector3 = Me.shadowTransform.position
			position.y = ground + FlyingCowboyLevelRicochetDebris.ShadowPositionOffset
			Me.shadowTransform.position = position
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060021E2 RID: 8674 RVA: 0x0013B9A4 File Offset: 0x00139DA4
	Private Sub shootRicochetProjectile()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim num As Single = MathUtils.DirectionToAngle([next].transform.position - MyBase.transform.position)
		Dim num2 As Integer
		Dim basicProjectile As BasicProjectile
		If Me.bulletParryable Then
			num2 = Global.UnityEngine.Random.Range(0, Me.parryableProjectiles.Length)
			basicProjectile = Me.parryableProjectiles(num2).Create(MyBase.transform.position, num, Me.bulletSpeed)
			num2 += 1
		Else
			num2 = Global.UnityEngine.Random.Range(0, Me.regularProjectiles.Length)
			basicProjectile = Me.regularProjectiles(num2).Create(MyBase.transform.position, num, Me.bulletSpeed)
		End If
		basicProjectile.SetParryable(Me.bulletParryable)
		basicProjectile.GetComponent(Of SpriteRenderer)().sortingOrder = num2
	End Sub

	' Token: 0x060021E3 RID: 8675 RVA: 0x0013BA74 File Offset: 0x00139E74
	Protected Overrides Sub Die()
		Me.RandomizeVariant()
		MyBase.Die()
		Dim flag As Boolean = Rand.Bool()
		Dim num As Integer
		If FlyingCowboyLevelRicochetDebris.LastBitsIndex = 0 Then
			num = If((Not flag), 2, 1)
		ElseIf FlyingCowboyLevelRicochetDebris.LastBitsIndex = 1 Then
			num = If((Not flag), 2, 0)
		Else
			num = If((Not flag), 1, 0)
		End If
		FlyingCowboyLevelRicochetDebris.LastBitsIndex = num
		For i As Integer = 0 To Me.deathBits.Length - 1
			Me.deathBits(i).enabled = i = num
		Next
		Me.deathEffect.Create(New Vector3(MyBase.transform.position.x, -360F + FlyingCowboyLevelRicochetDebris.GroundOffset - 10F))
	End Sub

	' Token: 0x060021E4 RID: 8676 RVA: 0x0013BB41 File Offset: 0x00139F41
	Private Sub SFX_COWGIRL_COWGIRL_P2_SafeDropImpact()
		AudioManager.Play("sfx_dlc_cowgirl_p2_safedropimpact")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_safedropimpact")
	End Sub

	' Token: 0x060021E5 RID: 8677 RVA: 0x0013BB5D File Offset: 0x00139F5D
	Private Sub SFX_COWGIRL_COWGIRL_P2_SafeHitPlayer()
		AudioManager.Play("sfx_dlc_cowgirl_p2_safehitplayer")
		Me.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_safehitplayer")
	End Sub

	' Token: 0x04002A94 RID: 10900
	Private Shared GroundOffset As Single = 100F

	' Token: 0x04002A95 RID: 10901
	Private Shared ShadowPositionOffset As Single = -50F

	' Token: 0x04002A96 RID: 10902
	Private Shared ShadowStartOffset As Single = 300F

	' Token: 0x04002A97 RID: 10903
	Private Shared ShadowEndOffset As Single = 50F

	' Token: 0x04002A98 RID: 10904
	Private Shared ShadowScaleRange As MinMax = New MinMax(0.1F, 1F)

	' Token: 0x04002A99 RID: 10905
	Private Shared AllowedRotations As Single() = New Single() { -20F, -10F, 0F, 10F, 20F }

	' Token: 0x04002A9A RID: 10906
	Private Shared LastBitsIndex As Integer = 0

	' Token: 0x04002A9B RID: 10907
	<SerializeField()>
	Private deathBits As SpriteRenderer()

	' Token: 0x04002A9C RID: 10908
	<SerializeField()>
	Private regularProjectiles As BasicProjectile()

	' Token: 0x04002A9D RID: 10909
	<SerializeField()>
	Private parryableProjectiles As BasicProjectile()

	' Token: 0x04002A9E RID: 10910
	<SerializeField()>
	Private shadowTransform As Transform

	' Token: 0x04002A9F RID: 10911
	<SerializeField()>
	Private deathEffect As Effect

	' Token: 0x04002AA0 RID: 10912
	Private bulletType As FlyingCowboyLevelRicochetDebris.BulletType

	' Token: 0x04002AA1 RID: 10913
	Private bulletSpeed As Single

	' Token: 0x04002AA2 RID: 10914
	Private bulletParryable As Boolean

	' Token: 0x0200065A RID: 1626
	Public Enum BulletType
		' Token: 0x04002AA4 RID: 10916
		[Nothing]
		' Token: 0x04002AA5 RID: 10917
		Ricochet
	End Enum
End Class
