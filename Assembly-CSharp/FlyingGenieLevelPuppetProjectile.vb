Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000676 RID: 1654
Public Class FlyingGenieLevelPuppetProjectile
	Inherits BasicProjectile

	' Token: 0x1700039A RID: 922
	' (get) Token: 0x060022D4 RID: 8916 RVA: 0x00146FE8 File Offset: 0x001453E8
	Public ReadOnly Property minRadius As Single
		Get
			Return Me._minRadius
		End Get
	End Property

	' Token: 0x1700039B RID: 923
	' (get) Token: 0x060022D5 RID: 8917 RVA: 0x00146FF0 File Offset: 0x001453F0
	Public ReadOnly Property maxRadius As Single
		Get
			Return Me._maxRadius
		End Get
	End Property

	' Token: 0x060022D6 RID: 8918 RVA: 0x00146FF8 File Offset: 0x001453F8
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.spawn_spark_cr())
	End Sub

	' Token: 0x060022D7 RID: 8919 RVA: 0x00147010 File Offset: 0x00145410
	Private Iterator Function spawn_spark_cr() As IEnumerator
		Dim pattern As String() = New String() { "B", "P", "B", "P", "P", "B", "P", "B", "B", "P", "B", "P" }
		Dim patternIndex As Integer = Global.UnityEngine.Random.Range(0, pattern.Length)
		While True
			Dim vector As Vector2 = MyBase.baseTransform.position
			Dim vector2 As Vector2 = New Vector2(Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)), Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)))
			Dim target As Vector2 = vector + vector2.normalized * Global.UnityEngine.Random.Range(Me.minRadius, Me.maxRadius)
			If pattern(patternIndex) = "B" Then
				Me.sparksBlue(Global.UnityEngine.Random.Range(0, Me.sparksBlue.Length)).Create(target)
			Else
				Me.sparksPink(Global.UnityEngine.Random.Range(0, Me.sparksPink.Length)).Create(target)
			End If
			patternIndex = (patternIndex + 1) Mod pattern.Length
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.08F, 0.2F))
		End While
		Return
	End Function

	' Token: 0x060022D8 RID: 8920 RVA: 0x0014702C File Offset: 0x0014542C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(1F, 0F, 0F, 1F)
		Gizmos.DrawWireSphere(MyBase.baseTransform.position, Me.minRadius)
		Gizmos.color = New Color(0F, 0F, 1F, 1F)
		Gizmos.DrawWireSphere(MyBase.baseTransform.position, Me.maxRadius)
	End Sub

	' Token: 0x060022D9 RID: 8921 RVA: 0x001470BB File Offset: 0x001454BB
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x04002B74 RID: 11124
	<SerializeField()>
	Private _minRadius As Single = 100F

	' Token: 0x04002B75 RID: 11125
	<SerializeField()>
	Private _maxRadius As Single = 200F

	' Token: 0x04002B76 RID: 11126
	<SerializeField()>
	Private sparksBlue As Effect()

	' Token: 0x04002B77 RID: 11127
	<SerializeField()>
	Private sparksPink As Effect()
End Class
