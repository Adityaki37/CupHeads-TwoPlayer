Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000490 RID: 1168
Public Class LevelHUDWeapon
	Inherits AbstractMonoBehaviour

	' Token: 0x0600125E RID: 4702 RVA: 0x000A9F38 File Offset: 0x000A8338
	Public Function Create(parent As Transform, weapon As Weapon) As LevelHUDWeapon
		Dim levelHUDWeapon As LevelHUDWeapon = Me.InstantiatePrefab(Of LevelHUDWeapon)()
		levelHUDWeapon.transform.SetParent(parent, False)
		levelHUDWeapon.SetIcon(weapon)
		Return levelHUDWeapon
	End Function

	' Token: 0x14000030 RID: 48
	' (add) Token: 0x0600125F RID: 4703 RVA: 0x000A9F64 File Offset: 0x000A8364
	' (remove) Token: 0x06001260 RID: 4704 RVA: 0x000A9F98 File Offset: 0x000A8398
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Private Shared Event OnAwakeEvent As Action

	' Token: 0x06001261 RID: 4705 RVA: 0x000A9FCC File Offset: 0x000A83CC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.startY = -80F
		Me.endY = -10F
		MyBase.transform.ResetLocalTransforms()
		MyBase.transform.SetLocalPosition(New Single?(0F), New Single?(Me.startY), New Single?(0F))
		Me.inCoroutine = MyBase.StartCoroutine(Me.go_cr())
		If LevelHUDWeapon.OnAwakeEvent IsNot Nothing Then
			LevelHUDWeapon.OnAwakeEvent()
		End If
		LevelHUDWeapon.OnAwakeEvent = Nothing
		AddHandler LevelHUDWeapon.OnAwakeEvent, AddressOf Me.Out
	End Sub

	' Token: 0x06001262 RID: 4706 RVA: 0x000AA067 File Offset: 0x000A8467
	Private Sub OnDestroy()
		RemoveHandler LevelHUDWeapon.OnAwakeEvent, AddressOf Me.Out
	End Sub

	' Token: 0x06001263 RID: 4707 RVA: 0x000AA07A File Offset: 0x000A847A
	Private Sub Out()
		If Not Me.ending Then
			If Me.inCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.inCoroutine)
			End If
			MyBase.StartCoroutine(Me.out_cr())
		End If
	End Sub

	' Token: 0x06001264 RID: 4708 RVA: 0x000AA0AB File Offset: 0x000A84AB
	Private Sub SetIcon(weapon As Weapon)
		MyBase.animator.Play(weapon.ToString())
	End Sub

	' Token: 0x06001265 RID: 4709 RVA: 0x000AA0C8 File Offset: 0x000A84C8
	Private Iterator Function go_cr() As IEnumerator
		Yield MyBase.TweenLocalPositionY(Me.startY, Me.endY, 0.2F, EaseUtils.EaseType.easeOutSine)
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Me.ending = True
		Yield MyBase.TweenLocalPositionY(Me.endY, Me.startY, 0.2F, EaseUtils.EaseType.easeInSine)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06001266 RID: 4710 RVA: 0x000AA0E4 File Offset: 0x000A84E4
	Private Iterator Function out_cr() As IEnumerator
		Me.ending = True
		Yield MyBase.TweenLocalPositionY(Me.endY, Me.startY, 0.05F, EaseUtils.EaseType.easeInSine)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04001BCC RID: 7116
	Public Const TIME_IN As Single = 0.2F

	' Token: 0x04001BCD RID: 7117
	Public Const TIME_DELAY As Single = 2F

	' Token: 0x04001BCE RID: 7118
	Public Const TIME_OUT As Single = 0.2F

	' Token: 0x04001BCF RID: 7119
	Public Const TIME_OUT_FAST As Single = 0.05F

	' Token: 0x04001BD0 RID: 7120
	Private ending As Boolean

	' Token: 0x04001BD1 RID: 7121
	Private inCoroutine As Coroutine

	' Token: 0x04001BD2 RID: 7122
	Private startY As Single

	' Token: 0x04001BD3 RID: 7123
	Private endY As Single
End Class
