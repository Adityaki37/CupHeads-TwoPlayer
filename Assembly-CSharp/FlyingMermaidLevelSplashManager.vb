Imports System
Imports UnityEngine

' Token: 0x0200069F RID: 1695
Public Class FlyingMermaidLevelSplashManager
	Inherits AbstractPausableComponent

	' Token: 0x170003A6 RID: 934
	' (get) Token: 0x060023E9 RID: 9193 RVA: 0x0015166C File Offset: 0x0014FA6C
	Public Shared ReadOnly Property Instance As FlyingMermaidLevelSplashManager
		Get
			If FlyingMermaidLevelSplashManager.splashManager Is Nothing Then
				FlyingMermaidLevelSplashManager.splashManager = New GameObject() With { .name = "SplashManager" }.AddComponent(Of FlyingMermaidLevelSplashManager)()
			End If
			Return FlyingMermaidLevelSplashManager.splashManager
		End Get
	End Property

	' Token: 0x060023EA RID: 9194 RVA: 0x001516AA File Offset: 0x0014FAAA
	Protected Overrides Sub Awake()
		MyBase.Awake()
		FlyingMermaidLevelSplashManager.splashManager = Me
	End Sub

	' Token: 0x060023EB RID: 9195 RVA: 0x001516B8 File Offset: 0x0014FAB8
	Private Sub OnTriggerEnter2D(collider As Collider2D)
		If collider.gameObject.tag = "EnemyProjectile" AndAlso collider.gameObject.GetComponent(Of FlyingMermaidLevelNoSplashMarker)() Is Nothing Then
			If collider.GetComponent(Of Collider2D)().bounds.size.x > 200F Then
				Me.SpawnMegaSplashMedium(collider.gameObject, 0F, False, 0F)
			ElseIf collider.GetComponent(Of Collider2D)().bounds.size.x > 50F Then
				Me.SpawnSplashMedium(collider.gameObject, 0F, False, 0F)
			Else
				Me.SpawnSplashSmall(collider.gameObject)
			End If
		End If
	End Sub

	' Token: 0x060023EC RID: 9196 RVA: 0x00151783 File Offset: 0x0014FB83
	Public Sub SpawnMegaSplashLarge(gameObject As GameObject, Optional extraX As Single = 0F, Optional overrideY As Boolean = False, Optional y As Single = 0F)
		Me.CreateSplash(Me.MegasplashLarge, gameObject, extraX, overrideY, y)
	End Sub

	' Token: 0x060023ED RID: 9197 RVA: 0x00151796 File Offset: 0x0014FB96
	Public Sub SpawnMegaSplashMedium(gameObject As GameObject, Optional extraX As Single = 0F, Optional overrideY As Boolean = False, Optional y As Single = 0F)
		Me.CreateSplash(Me.MegasplashMedium, gameObject, extraX, overrideY, y)
	End Sub

	' Token: 0x060023EE RID: 9198 RVA: 0x001517A9 File Offset: 0x0014FBA9
	Public Sub SpawnSplashMedium(gameObject As GameObject, Optional extraX As Single = 0F, Optional overrideY As Boolean = False, Optional y As Single = 0F)
		Me.CreateSplash(Me.SplashMedium, gameObject, extraX, overrideY, y)
	End Sub

	' Token: 0x060023EF RID: 9199 RVA: 0x001517BC File Offset: 0x0014FBBC
	Public Sub SpawnSplashSmall(gameObject As GameObject)
		Me.CreateSplash(Me.SplashSmall, gameObject, 0F, False, 0F)
	End Sub

	' Token: 0x060023F0 RID: 9200 RVA: 0x001517D8 File Offset: 0x0014FBD8
	Private Sub CreateSplash(effect As Effect, gameObject As GameObject, Optional extraX As Single = 0F, Optional overrideY As Boolean = False, Optional y As Single = 0F)
		Dim num As Single = 0F
		If gameObject.GetComponent(Of Renderer)() IsNot Nothing Then
			num = gameObject.GetComponent(Of Renderer)().bounds.size.y / 4F
		End If
		Dim vector As Vector3 = New Vector3(gameObject.transform.position.x + extraX, gameObject.transform.position.y - num)
		If overrideY Then
			vector.y = y
		End If
		Dim effect2 As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(effect)
		effect2.transform.position = vector
		If gameObject.GetComponent(Of SpriteRenderer)() IsNot Nothing Then
			effect2.GetComponent(Of SpriteRenderer)().sortingLayerName = gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName
			effect2.GetComponent(Of SpriteRenderer)().sortingOrder = gameObject.GetComponent(Of SpriteRenderer)().sortingOrder + 1
		End If
	End Sub

	' Token: 0x060023F1 RID: 9201 RVA: 0x001518B7 File Offset: 0x0014FCB7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.MegasplashLarge = Nothing
		Me.MegasplashMedium = Nothing
		Me.SplashMedium = Nothing
		Me.SplashSmall = Nothing
	End Sub

	' Token: 0x04002CB4 RID: 11444
	Public Shared splashManager As FlyingMermaidLevelSplashManager

	' Token: 0x04002CB5 RID: 11445
	Public spawnRootFront As Transform

	' Token: 0x04002CB6 RID: 11446
	Public spawnRootBack As Transform

	' Token: 0x04002CB7 RID: 11447
	<SerializeField()>
	Private MegasplashLarge As Effect

	' Token: 0x04002CB8 RID: 11448
	<SerializeField()>
	Private MegasplashMedium As Effect

	' Token: 0x04002CB9 RID: 11449
	<SerializeField()>
	Private SplashMedium As Effect

	' Token: 0x04002CBA RID: 11450
	<SerializeField()>
	Private SplashSmall As Effect
End Class
