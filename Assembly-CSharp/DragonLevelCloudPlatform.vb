Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005EC RID: 1516
Public Class DragonLevelCloudPlatform
	Inherits LevelPlatform

	' Token: 0x06001E12 RID: 7698 RVA: 0x00114C20 File Offset: 0x00113020
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.animator.SetInteger("Cloud", Global.UnityEngine.Random.Range(0, 3))
		Me.minX = -640F - MyBase.GetComponent(Of SpriteRenderer)().bounds.size.x / 2F
		Me.maxX = 640F + MyBase.GetComponent(Of SpriteRenderer)().bounds.size.x / 2F
	End Sub

	' Token: 0x06001E13 RID: 7699 RVA: 0x00114CA4 File Offset: 0x001130A4
	Public Overrides Sub AddChild(player As Transform)
		MyBase.AddChild(player)
		MyBase.animator.SetBool("HasPlayer", True)
	End Sub

	' Token: 0x06001E14 RID: 7700 RVA: 0x00114CBE File Offset: 0x001130BE
	Public Overrides Sub OnPlayerExit(player As Transform)
		MyBase.OnPlayerExit(player)
		If MyBase.players.Count <= 0 Then
			MyBase.animator.SetBool("HasPlayer", False)
		End If
	End Sub

	' Token: 0x06001E15 RID: 7701 RVA: 0x00114CE9 File Offset: 0x001130E9
	Private Sub OnDisable()
		Me.top.sprite = Nothing
	End Sub

	' Token: 0x06001E16 RID: 7702 RVA: 0x00114CF7 File Offset: 0x001130F7
	Public Sub GetProperties(manager As DragonLevelPlatformManager, properties As LevelProperties.Dragon.Clouds)
		Me.properties = properties
		Me.manager = manager
		Me.speed = If((Not properties.movingRight), properties.cloudSpeed, (-properties.cloudSpeed))
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001E17 RID: 7703 RVA: 0x00114D37 File Offset: 0x00113137
	Public Sub GetProperties(properties As LevelProperties.Dragon.Clouds, firstTime As Boolean)
		Me.properties = properties
		Me.speed = If((Not properties.movingRight), properties.cloudSpeed, (-properties.cloudSpeed))
		If firstTime Then
			MyBase.StartCoroutine(Me.move_cr())
		End If
	End Sub

	' Token: 0x06001E18 RID: 7704 RVA: 0x00114D78 File Offset: 0x00113178
	Private Iterator Function move_cr() As IEnumerator
		While True
			MyBase.transform.AddPosition(-DragonLevel.SPEED * Me.speed * CupheadTime.Delta, 0F, 0F)
			Yield Nothing
			If Me.properties.movingRight Then
				If MyBase.transform.position.x >= Me.maxX Then
					If Me.manager IsNot Nothing Then
						Me.manager.DestroyObjectPool(Me)
					Else
						Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
					End If
				End If
			ElseIf MyBase.transform.position.x <= Me.minX Then
				If Me.manager IsNot Nothing Then
					Me.manager.DestroyObjectPool(Me)
				Else
					Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
				End If
			End If
		End While
		Return
	End Function

	' Token: 0x040026DC RID: 9948
	<SerializeField()>
	Public top As SpriteRenderer

	' Token: 0x040026DD RID: 9949
	Private minX As Single

	' Token: 0x040026DE RID: 9950
	Private maxX As Single

	' Token: 0x040026DF RID: 9951
	Private properties As LevelProperties.Dragon.Clouds

	' Token: 0x040026E0 RID: 9952
	Private manager As DragonLevelPlatformManager

	' Token: 0x040026E1 RID: 9953
	Private speed As Single
End Class
