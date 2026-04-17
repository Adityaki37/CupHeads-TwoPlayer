Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000729 RID: 1833
Public Class PirateLevelSquidInkOverlay
	Inherits LevelProperties.Pirate.Entity

	' Token: 0x170003D3 RID: 979
	' (get) Token: 0x060027F3 RID: 10227 RVA: 0x001758C4 File Offset: 0x00173CC4
	' (set) Token: 0x060027F4 RID: 10228 RVA: 0x001758CB File Offset: 0x00173CCB
	Public Shared Property Current As PirateLevelSquidInkOverlay

	' Token: 0x170003D4 RID: 980
	' (get) Token: 0x060027F5 RID: 10229 RVA: 0x001758D4 File Offset: 0x00173CD4
	' (set) Token: 0x060027F6 RID: 10230 RVA: 0x001758F4 File Offset: 0x00173CF4
	Private Property alpha As Single
		Get
			Return Me.spriteRenderer.color.a
		End Get
		Set(value As Single)
			Me.color.a = Mathf.Clamp(value, 0F, 1F)
			Me.spriteRenderer.color = Me.color
		End Set
	End Property

	' Token: 0x060027F7 RID: 10231 RVA: 0x00175924 File Offset: 0x00173D24
	Protected Overrides Sub Awake()
		MyBase.Awake()
		PirateLevelSquidInkOverlay.Current = Me
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Me.spriteRenderer.enabled = False
		Me.alpha = 0F
		Me.color = Me.spriteRenderer.color
		Me.splatGroups = New List(Of PirateLevelSquidInkOverlay.SplatGroup)()
		Dim enumerator As IEnumerator = MyBase.transform.GetEnumerator()
		Try
			While enumerator.MoveNext()
				Dim obj As Object = enumerator.Current
				Dim transform As Transform = CType(obj, Transform)
				If transform.name.ToLower().Contains("group") Then
					Me.splatGroups.Add(New PirateLevelSquidInkOverlay.SplatGroup(transform))
				End If
			End While
		Finally
			Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
			Dim disposable2 As IDisposable = disposable
			If disposable IsNot Nothing Then
				disposable2.Dispose()
			End If
		End Try
	End Sub

	' Token: 0x060027F8 RID: 10232 RVA: 0x001759F8 File Offset: 0x00173DF8
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		PirateLevelSquidInkOverlay.Current = Nothing
		Me.smallSplat = Nothing
		Me.largeSplat = Nothing
	End Sub

	' Token: 0x060027F9 RID: 10233 RVA: 0x00175A14 File Offset: 0x00173E14
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Dim enumerator As IEnumerator = MyBase.baseTransform.GetEnumerator()
		Try
			While enumerator.MoveNext()
				Dim obj As Object = enumerator.Current
				Dim transform As Transform = CType(obj, Transform)
				If transform.gameObject.activeInHierarchy Then
					Dim enumerator2 As IEnumerator = transform.GetEnumerator()
					Try
						While enumerator2.MoveNext()
							Dim obj2 As Object = enumerator2.Current
							Dim transform2 As Transform = CType(obj2, Transform)
							If transform2.name.ToLower().Contains("small") Then
								Gizmos.DrawWireSphere(transform2.position, 20F)
							ElseIf transform2.name.ToLower().Contains("large") Then
								Gizmos.DrawWireSphere(transform2.position, 40F)
							End If
						End While
					Finally
						Dim disposable As IDisposable = TryCast(enumerator2, IDisposable)
						Dim disposable2 As IDisposable = disposable
						If disposable IsNot Nothing Then
							disposable2.Dispose()
						End If
					End Try
				End If
			End While
		Finally
			Dim disposable3 As IDisposable = TryCast(enumerator, IDisposable)
			Dim disposable4 As IDisposable = disposable3
			If disposable3 IsNot Nothing Then
				disposable4.Dispose()
			End If
		End Try
	End Sub

	' Token: 0x060027FA RID: 10234 RVA: 0x00175B30 File Offset: 0x00173F30
	Public Overrides Sub LevelInit(properties As LevelProperties.Pirate)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x060027FB RID: 10235 RVA: 0x00175B3C File Offset: 0x00173F3C
	Public Sub Hit()
		If Not Me.SFXSplatScreenActive Then
			AudioManager.Play("level_pirate_squid_blackout_screen")
			Me.SFXSplatScreenActive = True
		End If
		Dim squid As LevelProperties.Pirate.Squid = MyBase.properties.CurrentState.squid
		Me.spriteRenderer.enabled = True
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.splats_cr())
		MyBase.StartCoroutine(Me.hit_cr(squid))
	End Sub

	' Token: 0x060027FC RID: 10236 RVA: 0x00175BA4 File Offset: 0x00173FA4
	Private Iterator Function splats_cr() As IEnumerator
		Dim group As PirateLevelSquidInkOverlay.SplatGroup = Me.splatGroups(Global.UnityEngine.Random.Range(0, Me.splatGroups.Count))
		group.RandomizeDelay(10)
		For i As Integer = 0 To 10 - 1
			For Each splat As PirateLevelSquidInkOverlay.SplatGroup.Splat In group.splats
				If splat.delay = i Then
					Dim vector As Vector3 = splat.position
					If splat.type = PirateLevelSquidInkOverlay.SplatGroup.Splat.Type.Large Then
						Me.largeSplat.Create(vector)
					Else
						Me.smallSplat.Create(vector)
					End If
				End If
			Next
			Yield CupheadTime.WaitForSeconds(Me, 0.025F)
		Next
		Return
	End Function

	' Token: 0x060027FD RID: 10237 RVA: 0x00175BC0 File Offset: 0x00173FC0
	Private Iterator Function hit_cr(p As LevelProperties.Pirate.Squid) As IEnumerator
		If Not Me.SFXSplatScreenActive Then
			AudioManager.Play("level_pirate_squid_blackout_screen")
			Me.SFXSplatScreenActive = True
		End If
		Me.targetAlpha = Mathf.Clamp(Me.targetAlpha + p.opacityAdd, 0F, 1F)
		Dim t As Single = 0F
		While t < p.opacityAddTime
			Dim val As Single = t / p.opacityAddTime
			Me.alpha = Mathf.Lerp(Me.alpha, Me.targetAlpha, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, p.darkHoldTime)
		Yield MyBase.StartCoroutine(Me.fade_cr(p))
		Return
	End Function

	' Token: 0x060027FE RID: 10238 RVA: 0x00175BE4 File Offset: 0x00173FE4
	Private Iterator Function fade_cr(p As LevelProperties.Pirate.Squid) As IEnumerator
		Dim t As Single = 0F
		While t < p.darkFadeTime
			Dim val As Single = t / p.darkFadeTime
			Me.alpha = Mathf.Lerp(Me.alpha, 0F, val)
			Me.targetAlpha = Me.alpha
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.alpha = 0F
		Me.targetAlpha = Me.alpha
		Me.spriteRenderer.enabled = False
		Me.SFXSplatScreenActive = False
		Return
	End Function

	' Token: 0x040030B5 RID: 12469
	Private Const DELAY_MAX As Integer = 10

	' Token: 0x040030B6 RID: 12470
	Private Const DELAY_WAIT As Single = 0.025F

	' Token: 0x040030B8 RID: 12472
	<SerializeField()>
	Private largeSplat As Effect

	' Token: 0x040030B9 RID: 12473
	<SerializeField()>
	Private smallSplat As Effect

	' Token: 0x040030BA RID: 12474
	Private spriteRenderer As SpriteRenderer

	' Token: 0x040030BB RID: 12475
	Private splatGroups As List(Of PirateLevelSquidInkOverlay.SplatGroup)

	' Token: 0x040030BC RID: 12476
	Private SFXSplatScreenActive As Boolean

	' Token: 0x040030BD RID: 12477
	Private color As Color

	' Token: 0x040030BE RID: 12478
	Private targetAlpha As Single

	' Token: 0x0200072A RID: 1834
	Public Class SplatGroup
		' Token: 0x060027FF RID: 10239 RVA: 0x00175C08 File Offset: 0x00174008
		Public Sub New(parent As Transform)
			Me.splats = New List(Of PirateLevelSquidInkOverlay.SplatGroup.Splat)()
			Dim enumerator As IEnumerator = parent.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					Me.splats.Add(New PirateLevelSquidInkOverlay.SplatGroup.Splat(transform))
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End Sub

		' Token: 0x06002800 RID: 10240 RVA: 0x00175C84 File Offset: 0x00174084
		Public Sub RandomizeDelay(max As Integer)
			For Each splat As PirateLevelSquidInkOverlay.SplatGroup.Splat In Me.splats
				splat.delay = Global.UnityEngine.Random.Range(0, max)
			Next
		End Sub

		' Token: 0x040030BF RID: 12479
		Public splats As List(Of PirateLevelSquidInkOverlay.SplatGroup.Splat)

		' Token: 0x0200072B RID: 1835
		Public Class Splat
			' Token: 0x06002801 RID: 10241 RVA: 0x00175CE8 File Offset: 0x001740E8
			Public Sub New(transform As Transform)
				Me.position = transform.position
				If transform.name.ToLower().Contains("small") Then
					Me.type = PirateLevelSquidInkOverlay.SplatGroup.Splat.Type.Small
				Else
					Me.type = PirateLevelSquidInkOverlay.SplatGroup.Splat.Type.Large
				End If
			End Sub

			' Token: 0x040030C0 RID: 12480
			Public type As PirateLevelSquidInkOverlay.SplatGroup.Splat.Type

			' Token: 0x040030C1 RID: 12481
			Public position As Vector2

			' Token: 0x040030C2 RID: 12482
			Public delay As Integer

			' Token: 0x0200072C RID: 1836
			Public Enum Type
				' Token: 0x040030C4 RID: 12484
				Small
				' Token: 0x040030C5 RID: 12485
				Large
			End Enum
		End Class
	End Class
End Class
