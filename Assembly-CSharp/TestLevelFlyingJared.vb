Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004AC RID: 1196
Public Class TestLevelFlyingJared
	Inherits LevelProperties.Test.Entity

	' Token: 0x0600137E RID: 4990 RVA: 0x000ABA00 File Offset: 0x000A9E00
	Public Overrides Sub LevelInit(properties As LevelProperties.Test)
		MyBase.LevelInit(properties)
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.OnLevelStart
		AudioManager.PlayLoop("test_sound")
		Me.emitAudioFromObject.Add("test_sound")
	End Sub

	' Token: 0x0600137F RID: 4991 RVA: 0x000ABA67 File Offset: 0x000A9E67
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		MyBase.GetComponent(Of AudioWarble)().HandleWarble()
	End Sub

	' Token: 0x06001380 RID: 4992 RVA: 0x000ABA85 File Offset: 0x000A9E85
	Private Sub OnLevelStart()
		MyBase.StartCoroutine(Me.moveX_cr())
		MyBase.StartCoroutine(Me.moveY_cr())
		MyBase.StartCoroutine(Me.scale_cr())
	End Sub

	' Token: 0x06001381 RID: 4993 RVA: 0x000ABAB0 File Offset: 0x000A9EB0
	Private Function GetHealthTimeX() As Single
		Dim num As Single = 1F - MyBase.properties.CurrentHealth / MyBase.properties.TotalHealth
		Return MyBase.properties.CurrentState.moving.timeX.GetFloatAt(num)
	End Function

	' Token: 0x06001382 RID: 4994 RVA: 0x000ABAF8 File Offset: 0x000A9EF8
	Private Function GetHealthTimeY() As Single
		Dim num As Single = 1F - MyBase.properties.CurrentHealth / MyBase.properties.TotalHealth
		Return MyBase.properties.CurrentState.moving.timeY.GetFloatAt(num)
	End Function

	' Token: 0x06001383 RID: 4995 RVA: 0x000ABB40 File Offset: 0x000A9F40
	Private Function GetHealthTimeScale() As Single
		Dim num As Single = 1F - MyBase.properties.CurrentHealth / MyBase.properties.TotalHealth
		Return MyBase.properties.CurrentState.moving.timeScale.GetFloatAt(num)
	End Function

	' Token: 0x06001384 RID: 4996 RVA: 0x000ABB88 File Offset: 0x000A9F88
	Private Iterator Function moveX_cr() As IEnumerator
		Dim start As Single = MyBase.transform.position.x
		Dim [end] As Single = -start
		While True
			Me.childSprite.transform.SetScale(New Single?(1F), Nothing, Nothing)
			Yield MyBase.TweenLocalPositionX(start, [end], Me.GetHealthTimeX(), EaseUtils.EaseType.easeInOutSine)
			Me.childSprite.transform.SetScale(New Single?(-1F), Nothing, Nothing)
			Yield MyBase.TweenLocalPositionX([end], start, Me.GetHealthTimeX(), EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x06001385 RID: 4997 RVA: 0x000ABBA4 File Offset: 0x000A9FA4
	Private Iterator Function moveY_cr() As IEnumerator
		Dim start As Single = MyBase.transform.position.y
		Dim [end] As Single = start - 100F
		While True
			Yield MyBase.TweenLocalPositionY(start, [end], Me.GetHealthTimeY(), EaseUtils.EaseType.easeInOutSine)
			Yield MyBase.TweenLocalPositionY([end], start, Me.GetHealthTimeY(), EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x06001386 RID: 4998 RVA: 0x000ABBC0 File Offset: 0x000A9FC0
	Private Iterator Function scale_cr() As IEnumerator
		Dim start As Vector3 = New Vector3(1F, 1F, 1F)
		Dim [end] As Vector3 = New Vector3(2F, 2F, 2F)
		While True
			Yield MyBase.TweenScale(start, [end], Me.GetHealthTimeScale(), EaseUtils.EaseType.easeInOutSine)
			Yield MyBase.TweenScale([end], start, Me.GetHealthTimeScale(), EaseUtils.EaseType.easeInOutSine)
		End While
		Return
	End Function

	' Token: 0x04001C94 RID: 7316
	<SerializeField()>
	Private childSprite As Transform

	' Token: 0x04001C95 RID: 7317
	Private damageReceiver As DamageReceiver

	' Token: 0x04001C96 RID: 7318
	<SerializeField()>
	Private audioClip As AudioSource
End Class
