CREATE VIEW entreeunionsortie
(SELECT entree.identree as idmouvement,entree.date,entree.quantite,entree.idarticle,entree.idmagasin FROM entree
UNION
SELECT sortie.idsortie,mouvement.date,(-1*mouvement.quantite) as quantite,sortie.idarticle,sortie.idmagasin FROM mouvement 
JOIN sortie ON mouvement.idsortie = sortie.idsortie)
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
e.idmagasin
FROM
entreeunionsortie e
;


