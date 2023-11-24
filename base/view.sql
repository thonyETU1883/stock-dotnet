CREATE VIEW entreeunionsortie AS
(SELECT entree.identree as idmouvement,entree.date,entree.quantite,entree.idarticle,entree.idmagasin,entree.prixunitaire FROM entree
UNION
(SELECT mouvement.identree,mouvement.date,(-1*mouvement.quantite) as quantite,sortie.idarticle,sortie.idmagasin,sortie.prixunitaire FROM mouvement 
JOIN sortie ON mouvement.idsortie = sortie.idsortie))

ORDER BY DATE
;


CREATE VIEW mouvement_reste AS
SELECT
e.*,
COALESCE(
(e.quantite-(SELECT SUM(m.quantite) FROM mouvement m WHERE m.identree = e.identree)),e.quantite
) as reste
FROM entree e
;


CREATE VIEW etatdestock AS 
SELECT 
e.idmouvement,e.date,
(SELECT SUM(e1.quantite) FROM entreeunionsortie e1 WHERE e1.date <= e.date AND e1.idarticle = e.idarticle AND e1.idmagasin = e.idmagasin) as quantite,e.idarticle,
e.idmagasin,
(SELECT (SUM(e2.quantite*e2.prixunitaire)/SUM(quantite)) FROM entreeunionsortie e2 WHERE quantite>0 AND e2.date <= e.date AND e2.idarticle = e.idarticle AND e2.idmagasin = e.idmagasin JOIN article ON article.idunite = uniteequivalence.idunite) as prixunitaire
FROM
entreeunionsortie e
;



SELECT (SUM(e2.quantite*e2.prixunitaire)/SUM(quantite)) FROM entreeunionsortie e2 WHERE quantite>0 AND e2.date <= '2023-01-12' NAD ;