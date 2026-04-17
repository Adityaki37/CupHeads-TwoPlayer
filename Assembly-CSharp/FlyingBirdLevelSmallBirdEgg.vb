Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000629 RID: 1577
Public Class FlyingBirdLevelSmallBirdEgg
	Inherits AbstractCollidableObject

	' Token: 0x1700037F RID: 895
	' (get) Token: 0x0600201C RID: 8220 RVA: 0x0012769E File Offset: 0x00125A9E
	' (set) Token: 0x0600201D RID: 8221 RVA: 0x001276A6 File Offset: 0x00125AA6
	Public Property container As Transform

	' Token: 0x0600201E RID: 8222 RVA: 0x001276AF File Offset: 0x00125AAF
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.StartCoroutine(Me.animSpeed_cr())
	End Sub

	' Token: 0x0600201F RID: 8223 RVA: 0x001276CF File Offset: 0x00125ACF
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		Me.UpdateRotation()
	End Sub

	' Token: 0x06002020 RID: 8224 RVA: 0x001276ED File Offset: 0x00125AED
	Private Sub UpdateRotation()
	End Sub

	' Token: 0x06002021 RID: 8225 RVA: 0x001276EF File Offset: 0x00125AEF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002022 RID: 8226 RVA: 0x00127718 File Offset: 0x00125B18
	Public Sub SetParent(parent As Transform, properties As LevelProperties.FlyingBird)
		Me.properties = properties
		Me.container = New GameObject("Egg Container").transform
		Me.container.SetParent(parent)
		Me.container.ResetLocalPosition()
		MyBase.transform.SetParent(Me.container)
		MyBase.transform.ResetLocalTransforms()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002023 RID: 8227 RVA: 0x00127781 File Offset: 0x00125B81
	Public Sub Explode()
		MyBase.GetComponent(Of CircleCollider2D)().enabled = False
		MyBase.StartCoroutine(Me.explode_cr())
	End Sub

	' Token: 0x06002024 RID: 8228 RVA: 0x0012779C File Offset: 0x00125B9C
	Public Sub OnDeathAnimComplete()
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x06002025 RID: 8229 RVA: 0x001277AC File Offset: 0x00125BAC
	Private Iterator Function move_cr() As IEnumerator
		Yield MyBase.TweenLocalPositionX(0F, Me.properties.CurrentState.smallBird.eggRange.max, Me.properties.CurrentState.smallBird.eggMoveTime, EaseUtils.EaseType.easeOutCubic)
		While True
			Yield MyBase.TweenLocalPositionX(Me.properties.CurrentState.smallBird.eggRange.max, Me.properties.CurrentState.smallBird.eggRange.min, Me.properties.CurrentState.smallBird.eggMoveTime, EaseUtils.EaseType.easeInOutSine)
			Yield MyBase.TweenLocalPositionX(Me.properties.CurrentState.smallBird.eggRange.min, Me.properties.CurrentState.smallBird.eggRange.max, Me.properties.CurrentState.smallBird.eggMoveTime, EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x06002026 RID: 8230 RVA: 0x001277C8 File Offset: 0x00125BC8
	Private Iterator Function explode_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, CSng(Global.UnityEngine.Random.Range(0, 1)))
		MyBase.transform.SetLocalEulerAngles(New Single?(0F), New Single?(0F), New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		MyBase.animator.Play("Explode")
		Return
	End Function

	' Token: 0x06002027 RID: 8231 RVA: 0x001277E4 File Offset: 0x00125BE4
	Private Iterator Function animSpeed_cr() As IEnumerator
		Yield Nothing
		Return
	End Function

	' Token: 0x040028A1 RID: 10401
	Private damageDealer As DamageDealer

	' Token: 0x040028A2 RID: 10402
	Private properties As LevelProperties.FlyingBird
End Class
