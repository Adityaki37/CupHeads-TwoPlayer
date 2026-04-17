Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006D9 RID: 1753
Public Class MausoleumLevelSineGhost
	Inherits MausoleumLevelGhostBase

	' Token: 0x06002557 RID: 9559 RVA: 0x0015D5E0 File Offset: 0x0015B9E0
	Public Function Create(position As Vector2, rotation As Single, speed As Single, properties As LevelProperties.Mausoleum.SineGhost) As MausoleumLevelSineGhost
		Dim mausoleumLevelSineGhost As MausoleumLevelSineGhost = TryCast(MyBase.Create(position, rotation, speed), MausoleumLevelSineGhost)
		mausoleumLevelSineGhost.rotation = rotation
		mausoleumLevelSineGhost.properties = properties
		Return mausoleumLevelSineGhost
	End Function

	' Token: 0x06002558 RID: 9560 RVA: 0x0015D60C File Offset: 0x0015BA0C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.CalculateDirection()
		Me.CalculateSin()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002559 RID: 9561 RVA: 0x0015D630 File Offset: 0x0015BA30
	Private Sub CalculateSin()
		Dim vector As Vector2 = Vector2.zero
		vector = MathUtils.AngleToDirection(Me.rotation) / 2F
		Dim num As Single = -((vector.x - MyBase.transform.position.x) / (vector.y - MyBase.transform.position.y))
		Dim num2 As Single = vector.y - num * vector.x
		Dim zero As Vector2 = Vector2.zero
		zero.x = vector.x + 1F
		zero.y = num * zero.x + num2
		Me.normalized = Vector3.zero
		Me.normalized = zero - vector
		Me.normalized.Normalize()
	End Sub

	' Token: 0x0600255A RID: 9562 RVA: 0x0015D6FC File Offset: 0x0015BAFC
	Private Sub CalculateDirection()
		Dim vector As Vector2 = Vector2.zero
		vector = MathUtils.AngleToDirection(Me.rotation)
		Dim num As Single = Mathf.Atan2(vector.y, vector.x) * 57.29578F
		Me.pointAtTarget = MathUtils.AngleToDirection(num)
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(num))
	End Sub

	' Token: 0x0600255B RID: 9563 RVA: 0x0015D76C File Offset: 0x0015BB6C
	Private Iterator Function move_cr() As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		While True
			Me.angle += Me.properties.waveSpeed * CupheadTime.Delta
			If CupheadTime.Delta IsNot 0F Then
				pos += Me.normalized * Mathf.Sin(Me.angle + Me.properties.waveAmount) * (Me.properties.waveAmount / 2F)
			End If
			pos += Me.pointAtTarget * Me.properties.ghostSpeed * CupheadTime.Delta
			MyBase.transform.position = pos
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600255C RID: 9564 RVA: 0x0015D788 File Offset: 0x0015BB88
	Protected Overrides Sub Die()
		If Not MyBase.isDead Then
			Dim spriteDeathParts As SpriteDeathParts = Me.hat.CreatePart(MyBase.transform.position)
			spriteDeathParts.animator.SetBool("HatA", Rand.Bool())
		End If
		MyBase.Die()
	End Sub

	' Token: 0x0600255D RID: 9565 RVA: 0x0015D7D2 File Offset: 0x0015BBD2
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.hat = Nothing
	End Sub

	' Token: 0x04002DEA RID: 11754
	<SerializeField()>
	Private hat As SpriteDeathParts

	' Token: 0x04002DEB RID: 11755
	Private pointAtTarget As Vector3

	' Token: 0x04002DEC RID: 11756
	Private normalized As Vector3

	' Token: 0x04002DED RID: 11757
	Private rotation As Single

	' Token: 0x04002DEE RID: 11758
	Private angle As Single

	' Token: 0x04002DEF RID: 11759
	Private properties As LevelProperties.Mausoleum.SineGhost
End Class
