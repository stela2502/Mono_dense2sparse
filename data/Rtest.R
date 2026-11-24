library(Seurat)
da = read.delim('data/DenseMatrix.csv',sep="\t")

data = Read10X( "data/DenseMatrix/filtered_feature_bc_matrix")

a = as.matrix(da)
a = matrix(as.numeric(a), ncol=ncol(da))
sp = Matrix::Matrix( a, sparse=TRUE) 

rownames(sp) = rownames(data)

if ( length( all.equal( data, Matrix::Matrix( sp ) ) == 1 ) ){
	print ("OK")
}else {
	print ("FAIL")
}
