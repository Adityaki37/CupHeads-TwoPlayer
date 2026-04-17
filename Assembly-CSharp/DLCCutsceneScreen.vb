Imports System
Imports UnityEngine

' Token: 0x020003FB RID: 1019
Public Class DLCCutsceneScreen
	Inherits AbstractMonoBehaviour

	' Token: 0x06000DF0 RID: 3568 RVA: 0x00090ACD File Offset: 0x0008EECD
	Private Sub AniEvent_ShowText()
		Me.cutscene.ShowText()
	End Sub

	' Token: 0x06000DF1 RID: 3569 RVA: 0x00090ADA File Offset: 0x0008EEDA
	Private Sub AniEvent_ShowArrow()
		Me.cutscene.ShowArrow()
	End Sub

	' Token: 0x06000DF2 RID: 3570 RVA: 0x00090AE7 File Offset: 0x0008EEE7
	Private Sub AniEvent_IrisIn()
		Me.cutscene.IrisIn()
	End Sub

	' Token: 0x06000DF3 RID: 3571 RVA: 0x00090AF4 File Offset: 0x0008EEF4
	Private Sub AniEvent_IrisOut()
		Me.cutscene.IrisOut()
	End Sub

	' Token: 0x06000DF4 RID: 3572 RVA: 0x00090B01 File Offset: 0x0008EF01
	Private Sub AnimEvent_SFX_IntroStart_SeagullCall_1()
		AudioManager.Play("sfx_dlc_intro_seagullcall_1")
	End Sub

	' Token: 0x06000DF5 RID: 3573 RVA: 0x00090B0D File Offset: 0x0008EF0D
	Private Sub AnimEvent_SFX_IntroStart_SeagullCall_2()
		AudioManager.Play("sfx_dlc_intro_seagullcall_2")
	End Sub

	' Token: 0x06000DF6 RID: 3574 RVA: 0x00090B19 File Offset: 0x0008EF19
	Private Sub AnimEvent_SFX_IntroStart_SeagullCall_3()
		AudioManager.Play("sfx_dlc_intro_seagullcall_3")
	End Sub

	' Token: 0x06000DF7 RID: 3575 RVA: 0x00090B25 File Offset: 0x0008EF25
	Private Sub SFX_IntroStart_OceanAmbLoopStart()
		AudioManager.FadeSFXVolume("sfx_dlc_intro_oceanamb_loop", 0.5F, 1F)
	End Sub

	' Token: 0x06000DF8 RID: 3576 RVA: 0x00090B3B File Offset: 0x0008EF3B
	Private Sub SFX_IntroStart_OceanAmbLoopStop()
		AudioManager.FadeSFXVolume("sfx_dlc_intro_oceanamb_loop", 0F, 0.1F)
	End Sub

	' Token: 0x06000DF9 RID: 3577 RVA: 0x00090B51 File Offset: 0x0008EF51
	Private Sub AnimEvent_SFX_Intro_ChalliceAppear()
		AudioManager.Play("sfx_dlc_cutscene_intro_challiceappear")
	End Sub

	' Token: 0x06000DFA RID: 3578 RVA: 0x00090B5D File Offset: 0x0008EF5D
	Private Sub AnimEvent_SFX_Intro_Challiceglows()
		AudioManager.Play("sfx_dlc_cutscene_intro_challiceglows")
	End Sub

	' Token: 0x06000DFB RID: 3579 RVA: 0x00090B69 File Offset: 0x0008EF69
	Private Sub AnimEvent_SFX_Intro_EatCookie()
		AudioManager.Play("sfx_dlc_cutscene_intro_eatcookie")
	End Sub

	' Token: 0x06000DFC RID: 3580 RVA: 0x00090B75 File Offset: 0x0008EF75
	Private Sub AnimEvent_SFX_Intro_EnterBakery()
		AudioManager.Play("sfx_dlc_cutscene_intro_enterbakery")
	End Sub

	' Token: 0x06000DFD RID: 3581 RVA: 0x00090B81 File Offset: 0x0008EF81
	Private Sub AnimEvent_SFX_Intro_FollowChallice()
		AudioManager.Play("sfx_dlc_cutscene_intro_followchallice")
	End Sub

	' Token: 0x06000DFE RID: 3582 RVA: 0x00090B8D File Offset: 0x0008EF8D
	Private Sub AnimEvent_SFX_Intro_Recipeaccept()
		AudioManager.Play("sfx_dlc_cutscene_intro_recipeaccept")
	End Sub

	' Token: 0x06000DFF RID: 3583 RVA: 0x00090B99 File Offset: 0x0008EF99
	Private Sub AnimEvent_SFX_Intro_SaltBakerRecipe()
		AudioManager.Play("sfx_dlc_cutscene_intro_saltbakerrecipe")
	End Sub

	' Token: 0x06000E00 RID: 3584 RVA: 0x00090BA5 File Offset: 0x0008EFA5
	Private Sub AnimEvent_SFX_Intro_CookieMagic()
		AudioManager.Play("sfx_dlc_cutscene_intro_cookiemagic")
	End Sub

	' Token: 0x06000E01 RID: 3585 RVA: 0x00090BB1 File Offset: 0x0008EFB1
	Private Sub AnimEvent_SFX_Intro_FirstSwapGhost()
		AudioManager.Play("sfx_dlc_cutscene_intro_firstswapghost")
	End Sub

	' Token: 0x06000E02 RID: 3586 RVA: 0x00090BBD File Offset: 0x0008EFBD
	Private Sub AnimEvent_SFX_Intro_SecondSwapGhost()
		AudioManager.Play("sfx_dlc_cutscene_intro_secondswapghost")
	End Sub

	' Token: 0x06000E03 RID: 3587 RVA: 0x00090BC9 File Offset: 0x0008EFC9
	Private Sub AnimEvent_SFX_SaltBakerPre_ChaliceReveal()
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_chalicereveal")
	End Sub

	' Token: 0x06000E04 RID: 3588 RVA: 0x00090BD5 File Offset: 0x0008EFD5
	Private Sub AnimEvent_SFX_SaltBakerPre_EndLeanIn()
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_endleanin")
	End Sub

	' Token: 0x06000E05 RID: 3589 RVA: 0x00090BE1 File Offset: 0x0008EFE1
	Private Sub AnimEvent_SFX_SaltBakerPre_HelpClose()
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_helpclose")
	End Sub

	' Token: 0x06000E06 RID: 3590 RVA: 0x00090BED File Offset: 0x0008EFED
	Private Sub AnimEvent_SFX_SaltBakerPre_KnifeOakTableLol()
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_knifedefinitelyoaktable")
	End Sub

	' Token: 0x06000E07 RID: 3591 RVA: 0x00090BF9 File Offset: 0x0008EFF9
	Private Sub AnimEvent_SFX_SaltBakerPre_KnifeSwipe()
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_knifeswipe")
	End Sub

	' Token: 0x04001755 RID: 5973
	<SerializeField()>
	Protected cutscene As DLCGenericCutscene
End Class
