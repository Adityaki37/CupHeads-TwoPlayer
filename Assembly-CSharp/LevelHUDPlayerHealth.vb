Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200048D RID: 1165
Public Class LevelHUDPlayerHealth
	Inherits AbstractLevelHUDComponent

	' Token: 0x0600124D RID: 4685 RVA: 0x000A982E File Offset: 0x000A7C2E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.image = MyBase.GetComponent(Of Image)()
	End Sub

	' Token: 0x0600124E RID: 4686 RVA: 0x000A9842 File Offset: 0x000A7C42
	Public Overrides Sub Init(hud As LevelHUDPlayer)
		MyBase.Init(hud)
		Me.lastHealth = MyBase._player.stats.Health
		Me.OnHealthChanged(MyBase._player.stats.Health)
	End Sub

	' Token: 0x0600124F RID: 4687 RVA: 0x000A9878 File Offset: 0x000A7C78
	Public Sub OnHealthChanged(health As Integer)
		MyBase.animator.SetInteger("Health", Mathf.Clamp(health, 0, MyBase._player.stats.HealthMax))
		MyBase.animator.Play("Entry")
		If Me.lastHealth <> health Then
			Me.OnChangedHealth()
		End If
		Me.lastHealth = health
	End Sub

	' Token: 0x06001250 RID: 4688 RVA: 0x000A98D5 File Offset: 0x000A7CD5
	Private Sub OnChangedHealth()
		MyBase.TweenValue(0F, 1F, 0.3F, EaseUtils.EaseType.easeOutSine, AddressOf Me.ChangedHealthTween)
	End Sub

	' Token: 0x06001251 RID: 4689 RVA: 0x000A98FC File Offset: 0x000A7CFC
	Private Sub ChangedHealthTween(value As Single)
		Dim white As Color = Color.white
		Dim gray As Color = Color.gray
		MyBase.transform.localScale = Vector3.one * Mathf.Lerp(2F, 1F, value)
		Me.image.color = Color.Lerp(white, gray, value)
	End Sub

	' Token: 0x04001BBB RID: 7099
	Private Const HealthParameter As String = "Health"

	' Token: 0x04001BBC RID: 7100
	Private image As Image

	' Token: 0x04001BBD RID: 7101
	Private lastHealth As Integer
End Class
