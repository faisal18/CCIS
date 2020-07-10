
/*
INSERTING NEW PROVIDER TYPES 
*/
insert Into ccis..ProviderType ([Description], CreationDate)
select distinct [TYPE], GETDATE() from FS_WS_WSCTFW..LMUMasterProviders
where [type] not in (select [description] from ccis..ProviderType)


/*
INSERTING NEW PROVIDERS
*/


insert into ccis..Providers (ProviderUID, ProviderLicense,ProviderName, IsActive, ProviderTypeID,Emirate,[Source])
select 
[UID]
,FacilityLicense
, FacilityName


,(
case  [status]
when   'true' then 1 
when 'false' then 0 
end 
) as isActive
,(select top 1  isnull(ProviderTypeID,3) from ccis..ProviderType pt where pt.[Description] = LM.[Type]) as Providertype
--,*
,Emirate
,[Source]
--, PostOffice
 from FS_WS_WSCTFW..LMUMasterProviders LM
 where FacilityLicense not in (

select ProviderLicense from ccis..Providers
)


--select ProviderUID, ProviderLicense,ProviderName, * from ccis..Providers

