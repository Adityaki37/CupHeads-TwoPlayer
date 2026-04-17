Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000530 RID: 1328
Public Class ChessCastleLevelCloud
	Inherits AbstractPausableComponent

	' Token: 0x06001808 RID: 6152 RVA: 0x000D98F4 File Offset: 0x000D7CF4
	Public Sub Initialize(castle As ChessCastleLevel)
		Me.castle = castle
	End Sub

	' Token: 0x06001809 RID: 6153 RVA: 0x000D9900 File Offset: 0x000D7D00
	Private Sub Start()
		MyBase.animator.SetInteger("Version", Global.UnityEngine.Random.Range(0, 13))
		MyBase.animator.Update(0F)
		Dim bounds As Bounds = New Bounds(Vector3.zero, New Vector3(1280F, 720F, 0F) / Level.Current.CameraSettings.zoom)
		Dim vector As Vector2 = MyBase.GetComponent(Of SpriteRenderer)().sprite.bounds.size
		MyBase.transform.position = New Vector3(bounds.max.x + vector.x * 0.5F + 30F, Global.UnityEngine.Random.Range(bounds.min.y + vector.y * 0.5F + 30F, bounds.max.y - vector.y * 0.5F - 30F), Global.UnityEngine.Random.Range(0F, 1F))
		Me.speed = Me.speedRange.RandomFloat()
		MyBase.StartCoroutine(Me.destroy_cr())
	End Sub

	' Token: 0x0600180A RID: 6154 RVA: 0x000D9A34 File Offset: 0x000D7E34
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.castle = Nothing
	End Sub

	' Token: 0x0600180B RID: 6155 RVA: 0x000D9A44 File Offset: 0x000D7E44
	Private Sub Update()
		If Me.castle.rotating Then
			If Me.speedRampCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.speedRampCoroutine)
				Me.speedRampCoroutine = Nothing
			End If
			Me.speedMultiplier = Me.castle.rotationMultiplier
		ElseIf Me.wasRotating Then
			Me.speedRampCoroutine = MyBase.StartCoroutine(Me.speedRamp_cr())
		End If
		Me.wasRotating = Me.castle.rotating
		Dim num As Single = Me.speed * Me.speedMultiplier
		Dim position As Vector3 = MyBase.transform.position
		position.x -= num * CupheadTime.Delta
		MyBase.transform.position = position
	End Sub

	' Token: 0x0600180C RID: 6156 RVA: 0x000D9B04 File Offset: 0x000D7F04
	Private Iterator Function speedRamp_cr() As IEnumerator
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.35F
			Yield Nothing
			elapsedTime += Time.deltaTime
			Me.speedMultiplier = Mathf.Lerp(Me.castle.rotationMultiplier, 1F, elapsedTime / 0.35F)
		End While
		Me.speedMultiplier = 1F
		Me.speedRampCoroutine = Nothing
		Return
	End Function

	' Token: 0x0600180D RID: 6157 RVA: 0x000D9B20 File Offset: 0x000D7F20
	Private Iterator Function destroy_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 10F)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04002140 RID: 8512
	<SerializeField()>
	Private speedRange As MinMax

	' Token: 0x04002141 RID: 8513
	Private castle As ChessCastleLevel

	' Token: 0x04002142 RID: 8514
	Private speed As Single

	' Token: 0x04002143 RID: 8515
	Private speedMultiplier As Single = 1F

	' Token: 0x04002144 RID: 8516
	Private wasRotating As Boolean

	' Token: 0x04002145 RID: 8517
	Private speedRampCoroutine As Coroutine
End Class
