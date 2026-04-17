Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200063A RID: 1594
Public Class FlyingBlimpLevelGeminiShoot
	Inherits AbstractCollidableObject

	' Token: 0x060020B1 RID: 8369 RVA: 0x0012DC20 File Offset: 0x0012C020
	Public Sub Init(properties As LevelProperties.FlyingBlimp.Gemini, pos As Vector2)
		Me.properties = properties
		MyBase.transform.position = pos
		Me.smallRadius = MyBase.GetComponent(Of CircleCollider2D)().radius
		Dim num As Single = CSng(Global.UnityEngine.Random.Range(0, 2))
		Me.pointingUp = num = 0F
		If Me.pointingUp Then
			Me.projectileRoot = Me.projectileRootUp
		Else
			Me.projectileRoot = Me.projectileRootDown
		End If
		MyBase.StartCoroutine(Me.rotate_cr())
	End Sub

	' Token: 0x060020B2 RID: 8370 RVA: 0x0012DCAC File Offset: 0x0012C0AC
	Private Iterator Function rotate_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		AudioManager.Play("level_flying_blimp_wheel_start")
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.animator.SetBool("Attack", True)
		Me.smallFXSpawning = True
		MyBase.StartCoroutine(Me.spawn_small_fx_cr())
		AudioManager.PlayLoop("level_flying_blimp_gemini_sphere_attack")
		Dim pct As Single = 0F
		Dim startRotation As Single = CSng(If((Not Rand.Bool()), (-360), 360))
		While pct <= 1F
			MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(startRotation * pct))
			pct += CupheadTime.FixedDelta * Me.properties.rotationSpeed
			Me.ShootBullet()
			Yield wait
		End While
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(If((startRotation <> 360F), 360, (-360)))))
		Me.smallFXSpawning = False
		MyBase.animator.SetBool("Attack", False)
		MyBase.animator.SetTrigger("Leave")
		AudioManager.[Stop]("level_flying_blimp_gemini_sphere_attack")
		AudioManager.Play("level_flying_blimp_wheel_end")
		Return
	End Function

	' Token: 0x060020B3 RID: 8371 RVA: 0x0012DCC8 File Offset: 0x0012C0C8
	Private Sub ShootBullet()
		Dim num As Single = Me.projectileRoot.position.x - MyBase.transform.position.x
		Dim num2 As Single = Me.projectileRoot.position.y - MyBase.transform.position.y
		Dim num3 As Single = Mathf.Atan2(num2, num) * 57.29578F
		If Me.delayTime < Me.properties.bulletDelay Then
			Me.delayTime += 1F
		Else
			Me.projectilePrefab.Create(Me.projectileRoot.position, num3, Me.properties.bulletSpeed)
			Me.delayTime = 0F
		End If
	End Sub

	' Token: 0x060020B4 RID: 8372 RVA: 0x0012DD98 File Offset: 0x0012C198
	Private Iterator Function spawn_small_fx_cr() As IEnumerator
		While Me.smallFXSpawning
			Dim small As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.smallFX)
			Dim scale As Vector3 = New Vector3(1F, 1F, 1F)
			scale.x = If((Not Rand.Bool()), (-scale.x), scale.x)
			scale.y = If((Not Rand.Bool()), (-scale.y), scale.y)
			small.transform.SetScale(New Single?(scale.x), New Single?(scale.y), New Single?(1F))
			small.transform.eulerAngles = New Vector3(0F, 0F, Global.UnityEngine.Random.Range(0F, 360F))
			small.GetComponent(Of SpriteRenderer)().sortingOrder = Global.UnityEngine.Random.Range(0, 3)
			small.transform.position = Me.GetRandomPoint()
			MyBase.StartCoroutine(Me.delete_small_fx(small))
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		End While
		Return
	End Function

	' Token: 0x060020B5 RID: 8373 RVA: 0x0012DDB4 File Offset: 0x0012C1B4
	Private Function GetRandomPoint() As Vector2
		Dim vector As Vector2 = MyBase.transform.position
		Dim vector2 As Vector2 = New Vector2(CSng(Global.UnityEngine.Random.Range(-1, 1)), CSng(Global.UnityEngine.Random.Range(-1, 1)))
		Dim vector3 As Vector2 = vector2.normalized * (Me.smallRadius * Global.UnityEngine.Random.value) * 2F
		Return vector + vector3
	End Function

	' Token: 0x060020B6 RID: 8374 RVA: 0x0012DE14 File Offset: 0x0012C214
	Private Iterator Function delete_small_fx(smallFX As GameObject) As IEnumerator
		Yield smallFX.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "SmallFX", False, True)
		Global.UnityEngine.[Object].Destroy(smallFX)
		Return
	End Function

	' Token: 0x060020B7 RID: 8375 RVA: 0x0012DE36 File Offset: 0x0012C236
	Private Sub Die()
		AudioManager.Play("level_flying_blimp_gemini_sphere_leave")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002937 RID: 10551
	Private properties As LevelProperties.FlyingBlimp.Gemini

	' Token: 0x04002938 RID: 10552
	<SerializeField()>
	Private smallFX As GameObject

	' Token: 0x04002939 RID: 10553
	<SerializeField()>
	Private projectileRootUp As Transform

	' Token: 0x0400293A RID: 10554
	<SerializeField()>
	Private projectileRootDown As Transform

	' Token: 0x0400293B RID: 10555
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x0400293C RID: 10556
	Private smallRadius As Single

	' Token: 0x0400293D RID: 10557
	Private projectileRoot As Transform

	' Token: 0x0400293E RID: 10558
	Private target As Vector3

	' Token: 0x0400293F RID: 10559
	Private startRotation As Quaternion

	' Token: 0x04002940 RID: 10560
	Private rotationTime As Single

	' Token: 0x04002941 RID: 10561
	Private delayTime As Single

	' Token: 0x04002942 RID: 10562
	Private pointingUp As Boolean

	' Token: 0x04002943 RID: 10563
	Private smallFXSpawning As Boolean

	' Token: 0x04002944 RID: 10564
	Private halfWay As Boolean
End Class
